using Commons.Classes;
using Commons.Services;
using HealthAssistant.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthAssistant.Services
{
    public class DBService
    {
        string directory = AppDomain.CurrentDomain.BaseDirectory;

        string databaseName = "HealthAssistant.db";

        #region Get SQLite Version
        public string GetSQLiteVersion(string databasefolderpath, string databasename)
        {

            string cs = "Data Source=" + databasefolderpath + databasename;
            string stm = "SELECT SQLITE_VERSION()";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            return version;
        }
        #endregion

        #region Get All Tables From DB
        public List<string> GetAllTables()
        {
            List<string> result = new List<string>();
            using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name; ";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                result.Add(rdr.GetString(0));
            }

            return result;
        }
        #endregion

        #region Create Table
        public void CreateTable(string tablename, Type o)
        {
            using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            #region Standard
            if (tablename != "Workouts" && tablename != "Foodentries" && tablename != "Foods" && tablename != "Exercises" && tablename != "Bodyweights")
            {
                #region Get Values
                string columns = "(";
                var info = o.GetProperties();

                foreach (var prop in info)
                {
                    columns += prop.Name + " " + prop.PropertyType.Name + ",";
                }
                columns += "UNIQUE(";
                foreach (var prop in info)
                {
                    columns += prop.Name + ",";
                }
                columns = columns.Remove(columns.Length - 1);
                columns += "))";
                #endregion
                cmd.CommandText = @"Create table if not exists " + tablename + columns;
            }
            #endregion

            #region Sollwerte

            if (tablename == "Targetproteins" || tablename == "Targetcarbs" || tablename == "Targetfats")
            {
                cmd.CommandText = @"Create table if not exists " + tablename + " (target REAL)";
            }
            #endregion

            #region Workout
            else if (tablename == "Workouts")
            {
                cmd.CommandText = @"Create table if not exists " + tablename + "(workout text, date text, UNIQUE (workout, date))";
            }
            #endregion

            #region Food Entries or Foods
            else if (tablename == "Foodentries" || tablename == "Foods")
            {
                #region Get Values
                string columns = "( ID INTEGER PRIMARY KEY , ";
                var info = o.GetProperties();

                foreach (var prop in info)
                {
                    if (prop.Name != "ID")
                    {
                        columns += prop.Name + " " + prop.PropertyType.Name + ",";
                    }
                    else
                    {
                        //columns += prop.Name + " " + prop.PropertyType.Name + " PRIMARY KEY " + ",";
                    }

                }
                if (tablename == "Foods")
                {
                    columns += "UNIQUE(Name))";
                }
                else
                {
                    columns = columns.Remove(columns.Length - 1);
                    columns += ")";
                }

                #endregion
                cmd.CommandText = @"Create table if not exists " + tablename + columns;
            }
            #endregion

            #region Exercises
            else if (tablename == "Exercises")
            {
                #region Get Values
                string columns = "( ID INTEGER PRIMARY KEY , ";
                var info = o.GetProperties();

                foreach (var prop in info)
                {
                    if (prop.Name != "ID")
                    {
                        columns += prop.Name + " " + prop.PropertyType.Name + ",";
                    }
                    else
                    {
                        //columns += prop.Name + " " + prop.PropertyType.Name + " PRIMARY KEY " + ",";
                    }

                }
                columns += "UNIQUE(ExerciseName))";

                #endregion
                cmd.CommandText = @"Create table if not exists " + tablename + columns;
            }
            #endregion

            #region Bodyweight
            else if (tablename == "Bodyweights")
            {
                #region Get Values
                string columns = "( ID INTEGER PRIMARY KEY , ";
                var info = o.GetProperties();

                foreach (var prop in info)
                {
                    if (prop.Name != "ID")
                    {
                        columns += prop.Name + " " + prop.PropertyType.Name + ",";
                    }
                    else
                    {
                        //columns += prop.Name + " " + prop.PropertyType.Name + " PRIMARY KEY " + ",";
                    }

                }
                columns += "UNIQUE(Date))";

                #endregion
                cmd.CommandText = @"Create table if not exists " + tablename + columns;
            }
            #endregion

            cmd.ExecuteNonQuery();

            PersistentDataProvider.Current.allTableNames = PersistentDataProvider.Current.databaseService.GetAllTables();
        }
        #endregion

        #region Delete Table
        public void DeleteTable(string tablename)
        {
            if(!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = @"Drop table " + tablename;
            cmd.ExecuteNonQuery();

            PersistentDataProvider.Current.allTableNames = PersistentDataProvider.Current.databaseService.GetAllTables();
        }
        #endregion

        #region Insert into table
        public void InsertValues(string tablename, Type o, object obj)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                using var cmd = new SQLiteCommand(con);
                #region Standard
                if (tablename != "Workouts" && tablename != "Targetproteins" && tablename != "Targetcarbs" && tablename != "Targetfats")
                {

                    #region Get Values
                    string values = "(";
                    Type myType = obj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.Name != "ID")
                        {
                            object propValue = prop.GetValue(obj, null);
                            if (prop.PropertyType == typeof(string))
                            {
                                values += "'" + propValue.ToString() + "'" + ",";
                            }
                            else
                            {
                                values += propValue.ToString().Replace(",", ".") + ",";
                            }
                        }
                    }

                    values = values.Remove(values.Length - 1);
                    values += ")";
                    #endregion
                    #region Properties
                    string properties = "(";
                    var info = o.GetProperties();

                    foreach (var prop in info)
                    {
                        if (prop.Name != "ID")
                        {
                            properties += prop.Name + ",";
                        }

                    }
                    properties = properties.Remove(properties.Length - 1);
                    properties += ")";
                    #endregion
                    cmd.CommandText = "INSERT INTO " + tablename + properties + "VALUES" + values;

                }
                #endregion
                #region Workouts
                else if (tablename == "Workouts")
                {
                    if (((Workout)obj).ExerciseList.Count != 0)
                    {
                        SerializerService ser = new SerializerService();

                        string serializedExerciseList = ser.Serialize<ObservableCollection<ExerciseEntry>>(((Workout)obj).ExerciseList);

                        string values = "INSERT INTO Workouts(Workout, Date) VALUES ('" + serializedExerciseList + "' , '" + ((Workout)obj).Date.ToString() + " ') ";
                        cmd.CommandText = values;
                    }
                    else
                    {
                        cmd.CommandText = "";
                    }

                }
                #endregion
                #region TargetValues
                else if (tablename == "Targetproteins" || tablename == "Targetfats" || tablename == "Targetcarbs")
                {
                    cmd.CommandText = "INSERT INTO " + tablename + " (target) VALUES ( " + obj.ToString().Replace(",", ".") + " )";
                }
                #endregion

                cmd.ExecuteNonQuery();

                #region Daten aktualisieren
                if (tablename == "Foodentries")
                {
                    PersistentDataProvider.Current.FoodEntriesToday = new ObservableCollection<FoodEntry>(PersistentDataProvider.Current.databaseService.GetFoodEntriesOfToday("Foodentries"));

                }
                if (tablename == "Bodyweights")
                {
                    PersistentDataProvider.Current.AllWeights = new ObservableCollection<Bodyweight>(PersistentDataProvider.Current.databaseService.GetAllBodyweights("Bodyweights"));
                }
                if (tablename == "Foods")
                {
                    PersistentDataProvider.Current.AllFoods = new ObservableCollection<Food>(PersistentDataProvider.Current.databaseService.GetAllFoods("Foods"));
                }
                if (tablename == "Exercises")
                {
                    PersistentDataProvider.Current.AllExercises = new ObservableCollection<Exercise>(PersistentDataProvider.Current.databaseService.GetAllExercises("Exercises"));
                }
                if (tablename == "Workouts")
                {
                    PersistentDataProvider.Current.AllWorkouts = new ObservableCollection<Workout>(PersistentDataProvider.Current.databaseService.GetAllWorkouts("Workouts"));
                    PersistentDataProvider.Current.WorkoutDays = new ObservableCollection<DateTime>(PersistentDataProvider.Current.AllWorkouts.Select(x => x.Date));
                }
                if (tablename == "Targetproteins")
                {
                    PersistentDataProvider.Current.TargetProteins = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetproteins");
                }
                if (tablename == "Targetcarbs")
                {
                    PersistentDataProvider.Current.TargetCarbs = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetcarbs");
                }
                if (tablename == "Targetfats")
                {
                    PersistentDataProvider.Current.TargetFats = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetfats");
                }
                #endregion

            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                string s = ex.Message;
            }

        }
        #endregion

        #region Query Full Tables

        #region Food Query gesamt
        public List<Food> GetAllFoods(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                List<Food> result = new List<Food>();
                while (rdr.Read())
                {
                    Food b = new Food(rdr.GetString(1), rdr.GetDouble(2), rdr.GetDouble(3), rdr.GetDouble(4));
                    b.ID = rdr.GetInt32(0);
                    result.Add(b);
                }

                return result.OrderBy(x => x.Name).ToList();
            }

            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Table " + tablename + " not found!");
                return null;
            }


        }
        #endregion

        #region Get All Food Entries
        public List<FoodEntry> GetAllFoodEntries(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                List<FoodEntry> result = new List<FoodEntry>();
                while (rdr.Read())
                {
                    FoodEntry b = new FoodEntry(PersistentDataProvider.Current.AllFoods.Where(x => x.Name == rdr.GetString(1)).FirstOrDefault(), rdr.GetDouble(2), rdr.GetString(3));
                    b.ID = rdr.GetInt32(0);
                    result.Add(b);
                }

                return result.OrderBy(x => x.Date).ToList();
            }

            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Table " + tablename + " not found!");
                return null;
            }


        }
        #endregion

        #region Gewicht Query gesamt
        public List<Bodyweight> GetAllBodyweights(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                List<Bodyweight> result = new List<Bodyweight>();
                while (rdr.Read())
                {
                    Bodyweight b = new Bodyweight(rdr.GetString(1), rdr.GetDouble(2));
                    b.ID = rdr.GetInt32(0);
                    result.Add(b);
                }
                return result;
            }

            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Table " + tablename + " not found!");
                return null;
            }


        }
        #endregion

        #region Exercise Query gesamt
        public List<Exercise> GetAllExercises(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                List<Exercise> result = new List<Exercise>();
                while (rdr.Read())
                {
                    Exercise b = new Exercise(rdr.GetString(1), rdr.GetString(2));
                    b.ID = rdr.GetInt32(0);
                    result.Add(b);
                }
                return result;
            }

            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Table " + tablename + " not found!");
                return null;
            }


        }
        #endregion

        #region Workout Query gesamt
        public List<Workout> GetAllWorkouts(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                SerializerService ser = new SerializerService();
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                List<Workout> result = new List<Workout>();
                while (rdr.Read())
                {
                    Workout t = new Workout();
                    t.Date = Convert.ToDateTime(rdr.GetString(1));
                    t.ExerciseList = ser.Deserialize<ObservableCollection<ExerciseEntry>>(rdr.GetString(0));
                    result.Add(t);
                }
                return result;
            }

            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Table " + tablename + " not found!");
                return null;
            }


        }
        #endregion

        #region Sollwertequery gesamt
        public double GetTargetValue(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                List<double> result = new List<double>();
                while (rdr.Read())
                {
                    double t = rdr.GetDouble(0);
                    result.Add(t);
                }
                return result.Last();
            }

            catch (Exception ex)
            {
                return 0;
            }



        }
        #endregion

        #endregion

        #region FoodEntry Query filtered by today
        public List<FoodEntry> GetFoodEntriesOfToday(string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                List<Food> FoodHelperListe = GetAllFoods("Foods");
                List<FoodEntry> result = new List<FoodEntry>();

                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                string stm = "SELECT * FROM " + tablename;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    //string u = rdr.GetString(1);
                    Food helper = (from p in FoodHelperListe
                                           where p.Name == rdr.GetString(1)
                                           select p).FirstOrDefault();

                    FoodEntry c = new FoodEntry(helper, rdr.GetDouble(2), rdr.GetString(3));
                    c.ID = rdr.GetInt32(0);
                    result.Add(c);
                }
                var helper2 = result.AsEnumerable().Where(x => Convert.ToDateTime(x.Date) == DateTime.Today).OrderBy(x => x.Food).GroupBy(x => x.Food).ToList();


                List<FoodEntry> endresult = new List<FoodEntry>();
                foreach (var x in helper2)
                {
                    Food h = (from p in PersistentDataProvider.Current.AllFoods
                                      where p.Name == x.First().Food
                                      select p).FirstOrDefault();
                    double gewicht = x.Sum(x => x.Weight);
                    endresult.Add(new FoodEntry(h, gewicht, DateTime.Today.ToString()));
                }

                return endresult;
            }

            catch (System.Data.SQLite.SQLiteException)
            {
                return null;
            }


        }
        #endregion

        #region Delete Item from Table
        public void DeleteFromTable(string itemName, string propertyName, string tablename)
        {
            if (!PersistentDataProvider.Current.allTableNames.Contains(tablename))
            {
                MessageBox.Show($"Table {tablename} not found");
            }
            try
            {
                using var con = new SQLiteConnection("Data Source=" + directory + databaseName);
                con.Open();

                using var cmd = new SQLiteCommand(con);

                if (propertyName == "ID")
                {
                    cmd.CommandText = "DELETE FROM " + tablename + " WHERE " + propertyName + " LIKE " + itemName;
                }
                else
                {
                    cmd.CommandText = "DELETE FROM " + tablename + " WHERE " + propertyName + " LIKE " + "'%" + itemName + "%'";
                }

                cmd.ExecuteNonQuery();

                #region Daten aktualisieren
                if (tablename == "Foodentries")
                {
                    PersistentDataProvider.Current.FoodEntriesToday = new ObservableCollection<FoodEntry>(PersistentDataProvider.Current.databaseService.GetFoodEntriesOfToday("Foodentries"));
                }
                if (tablename == "Bodyweights")
                {
                    PersistentDataProvider.Current.AllWeights = new ObservableCollection<Bodyweight>(PersistentDataProvider.Current.databaseService.GetAllBodyweights("Bodyweights"));
                }
                if (tablename == "Foods")
                {
                    PersistentDataProvider.Current.AllFoods = new ObservableCollection<Food>(PersistentDataProvider.Current.databaseService.GetAllFoods("Foods"));
                }
                if (tablename == "Exercises")
                {
                    PersistentDataProvider.Current.AllExercises = new ObservableCollection<Exercise>(PersistentDataProvider.Current.databaseService.GetAllExercises("Exercises"));
                }
                if (tablename == "Workouts")
                {
                    PersistentDataProvider.Current.AllWorkouts = new ObservableCollection<Workout>(PersistentDataProvider.Current.databaseService.GetAllWorkouts("Workouts"));
                }
                if (tablename == "Targetproteins")
                {
                    PersistentDataProvider.Current.TargetProteins = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetproteins");
                }
                if (tablename == "Targetcarbs")
                {
                    PersistentDataProvider.Current.TargetCarbs = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetcarbs");
                }
                if (tablename == "Targetfats")
                {
                    PersistentDataProvider.Current.TargetFats = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetfats");
                }
                #endregion
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Keine Übereinstimmung gefunden");
            }
        }
        #endregion
    }
}
