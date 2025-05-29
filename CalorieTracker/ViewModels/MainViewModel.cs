using CalorieTracker.Models;
using CalorieTracker.Repositories;
using CalorieTracker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CalorieTracker.ViewModels
{
    public class MainViewModel : ViewModelBase, ICalorieGoalObserver
    {
        private readonly ProductRepository _productRepository;
        private readonly FoodEntryRepository _foodEntryRepository;
        private readonly RecipeRepository _recipeRepository;
        private readonly UserProfileRepository _userProfileRepository;
        private readonly ICalorieCalculatorService _calorieCalculatorService;
        private readonly CalorieGoalNotificationService _notificationService;
        private readonly CommandInvoker _commandInvoker;

        private DateTime _selectedDate = DateTime.Today;
        private double _dailyCalories;
        private double _dailyGoal;
        private string _goalStatus = string.Empty;
        private UserProfile _userProfile;

        public MainViewModel()
        {
            _productRepository = new ProductRepository();
            _productRepository.LoadData(); 
            _foodEntryRepository = new FoodEntryRepository(_productRepository);
            _foodEntryRepository.LoadData();
            _recipeRepository = new RecipeRepository(_productRepository);
            _userProfileRepository = new UserProfileRepository();
            _calorieCalculatorService = new CalorieCalculatorService(_foodEntryRepository, _userProfileRepository);
            _notificationService = new CalorieGoalNotificationService();
            _commandInvoker = new CommandInvoker();

            _userProfile = _userProfileRepository.GetProfile();
            _notificationService.Subscribe(this);

            InitializeCollections();
            InitializeCommands();
            UpdateDailyStats();
        }

        #region Properties

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    UpdateDailyStats();
                }
            }
        }

        public double DailyCalories
        {
            get => _dailyCalories;
            set => SetProperty(ref _dailyCalories, value);
        }

        public double DailyGoal
        {
            get => _dailyGoal;
            set => SetProperty(ref _dailyGoal, value);
        }

        public string GoalStatus
        {
            get => _goalStatus;
            set => SetProperty(ref _goalStatus, value);
        }

        public UserProfile UserProfile
        {
            get => _userProfile;
            set => SetProperty(ref _userProfile, value);
        }

        public ObservableCollection<FoodEntry> TodayEntries { get; } = new ObservableCollection<FoodEntry>();
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
        public ObservableCollection<Recipe> Recipes { get; } = new ObservableCollection<Recipe>();
        public ObservableCollection<DailyStats> WeeklyStats { get; } = new ObservableCollection<DailyStats>();
        public double WeeklyAverage => WeeklyStats.Any() ? WeeklyStats.Average(s => s.TotalCalories) : 0;
        public int DaysOnTarget => WeeklyStats.Count(s => s.TotalCalories <= DailyGoal);
        public string BestDay
        {
            get
            {
                if (!WeeklyStats.Any()) return "N/A";
                var bestDay = WeeklyStats.OrderByDescending(s => s.TotalCalories).First();
                return $"{bestDay.Date.DayOfWeek} ({bestDay.TotalCalories:F0} cal)";
            }
        }

        public double DailyProtein => TodayEntries.Sum(e => e.Product.Protein * e.Quantity / 100);
        public double ProteinGoal => UserProfile?.DailyProteinGoal ?? 0;
        public double DailyCarbs => TodayEntries.Sum(e => e.Product.Carbs * e.Quantity / 100);
        public double CarbsGoal => UserProfile?.DailyCarbsGoal ?? 0;
        public double DailyFat => TodayEntries.Sum(e => e.Product.Fat * e.Quantity / 100);
        public double FatGoal => UserProfile?.DailyFatGoal ?? 0;

        #endregion

        #region Commands

        public System.Windows.Input.ICommand AddFoodEntryCommand { get; private set; }
        public System.Windows.Input.ICommand DeleteFoodEntryCommand { get; private set; }
        public System.Windows.Input.ICommand AddProductCommand { get; private set; }
        public System.Windows.Input.ICommand EditProductCommand { get; private set; }
        public System.Windows.Input.ICommand DeleteProductCommand { get; private set; }
        public System.Windows.Input.ICommand SaveUserProfileCommand { get; private set; }
        public System.Windows.Input.ICommand UndoCommand { get; private set; }
        public System.Windows.Input.ICommand RefreshDataCommand { get; private set; }
        public System.Windows.Input.ICommand ShowPreviousDayCommand { get; private set; }
        public System.Windows.Input.ICommand ShowNextDayCommand { get; private set; }
        #endregion

        private void InitializeCommands()
        {
            AddFoodEntryCommand = new RelayCommand(AddFoodEntry);
            DeleteFoodEntryCommand = new RelayCommand(DeleteFoodEntry, CanDeleteFoodEntry);
            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand(EditProduct, CanEditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            SaveUserProfileCommand = new RelayCommand(SaveUserProfile);
            UndoCommand = new RelayCommand(UndoLastAction, CanUndo);
            RefreshDataCommand = new RelayCommand(RefreshData);
            ShowPreviousDayCommand = new RelayCommand(ShowPreviousDay);
            ShowNextDayCommand = new RelayCommand(ShowNextDay);
        }

        private void InitializeCollections()
        {
            RefreshProducts();
            RefreshRecipes();
        }

        private void UpdateDailyStats()
        {
            var stats = _calorieCalculatorService.GetDailyStats(SelectedDate);
            DailyCalories = stats.TotalCalories;
            DailyGoal = stats.Goal;

            var goalStatus = _calorieCalculatorService.GetCalorieGoalStatus(SelectedDate);
            GoalStatus = GetGoalStatusText(goalStatus);

            TodayEntries.Clear();
            foreach (var entry in stats.Entries)
            {
                TodayEntries.Add(entry);
            }

            UpdateWeeklyStats();

            OnPropertyChanged(nameof(WeeklyAverage));
            OnPropertyChanged(nameof(DaysOnTarget));
            OnPropertyChanged(nameof(BestDay));
            OnPropertyChanged(nameof(DailyProtein));
            OnPropertyChanged(nameof(DailyCarbs));
            OnPropertyChanged(nameof(DailyFat));
        }

        private void UpdateWeeklyStats()
        {
            var startOfWeek = SelectedDate.Date.AddDays(-(int)SelectedDate.DayOfWeek);
            var weeklyStats = _calorieCalculatorService.GetWeeklyStats(startOfWeek);

            WeeklyStats.Clear();
            foreach (var stat in weeklyStats)
            {
                WeeklyStats.Add(stat);
            }

            OnPropertyChanged(nameof(WeeklyAverage));
            OnPropertyChanged(nameof(DaysOnTarget));
            OnPropertyChanged(nameof(BestDay));
        }

        private string GetGoalStatusText(CalorieGoalStatus status)
        {
            switch (status.Status)
            {
                case GoalStatusType.UnderGoal:
                    return $"Under goal by {Math.Abs(status.Difference):F0} calories ({status.Percentage:F1}%)";
                case GoalStatusType.OnTrack:
                    return $"On track! ({status.Percentage:F1}% of goal)";
                case GoalStatusType.OverGoal:
                    return $"Over goal by {status.Difference:F0} calories ({status.Percentage:F1}%)";
                default:
                    return "Unknown status";
            }
        }

        #region Command Implementations

        private void AddFoodEntry(object parameter)
        {
            var entryViewModel = parameter as FoodEntryViewModel;
            if (entryViewModel == null) return;

            var foodEntry = new FoodEntry
            {
                Product = entryViewModel.SelectedProduct,
                Quantity = entryViewModel.Quantity,
                DateTime = SelectedDate,
                MealType = entryViewModel.MealType
            };

            var command = new AddFoodEntryCommand(_foodEntryRepository, foodEntry, _notificationService);
            _commandInvoker.ExecuteCommand(command);
        }

        private void DeleteFoodEntry(object parameter)
        {
            var entry = parameter as FoodEntry;
            if (entry == null) return;

            var command = new DeleteFoodEntryCommand(_foodEntryRepository, entry.Id, _notificationService);
            _commandInvoker.ExecuteCommand(command);
        }

        private bool CanDeleteFoodEntry(object parameter) => parameter is FoodEntry;

        private void AddProduct(object parameter)
        {
            var product = parameter as Product;
            if (product == null) return;

            _productRepository.Add(product);
            _productRepository.SaveChanges();
            RefreshProducts();
        }

        private void EditProduct(object parameter)
        {
            var product = parameter as Product;
            if (product == null) return;

            _productRepository.Update(product);
            _productRepository.SaveChanges();
            RefreshProducts();
        }

        private bool CanEditProduct(object parameter) => parameter is Product;

        private void DeleteProduct(object parameter)
        {
            var product = parameter as Product;
            if (product == null) return;

            _productRepository.Delete(product.Id);
            _productRepository.SaveChanges();
            RefreshProducts();
        }

        private bool CanDeleteProduct(object parameter) => parameter is Product;

        private void SaveUserProfile(object parameter)
        {
            _userProfileRepository.SaveProfile(UserProfile);
            _notificationService.NotifyCalorieGoalUpdated(UserProfile.DailyCalorieGoal);
        }

        private void UndoLastAction(object parameter)
        {
            _commandInvoker.UndoLastCommand();
        }

        private bool CanUndo(object parameter) => _commandInvoker.CanUndo;

        private void RefreshData(object parameter)
        {
            RefreshProducts();
            RefreshRecipes();
            UpdateDailyStats();
        }

        private void ShowPreviousDay(object parameter)
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }

        private void ShowNextDay(object parameter)
        {
            SelectedDate = SelectedDate.AddDays(1);
        }

        #endregion

        private void RefreshProducts()
        {
            Products.Clear();
            foreach (var product in _productRepository.GetAll())
            {
                Products.Add(product);
            }
        }

        private void RefreshRecipes()
        {
            Recipes.Clear();
            foreach (var recipe in _recipeRepository.GetAll())
            {
                Recipes.Add(recipe);
            }
        }

        #region ICalorieGoalObserver Implementation

        public void OnCalorieGoalUpdated(double newGoal)
        {
            DailyGoal = newGoal;
            UpdateDailyStats();
        }

        public void OnDailyCaloriesChanged(DateTime date, double totalCalories)
        {
            if (date.Date == SelectedDate.Date)
            {
                UpdateDailyStats();

                OnPropertyChanged(nameof(DailyProtein));
                OnPropertyChanged(nameof(DailyCarbs));
                OnPropertyChanged(nameof(DailyFat));
            }
        }
        #endregion
    }
}
