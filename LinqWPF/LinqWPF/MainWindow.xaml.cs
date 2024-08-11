using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinqWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //readonly SqlConnection conn;
        private readonly LinqToSQLClassesDataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();

            string connectiongString = ConfigurationManager.ConnectionStrings["LinqWPF.Properties.Settings.DBConnectionString"].ConnectionString;
            dataContext = new LinqToSQLClassesDataContext(connectiongString);

            //InsertUnis();
            //InsertStudents();
            //InsertLecture();
            //InsertStudentLectureAssociations();
            //GetUniOfCassi();
            //GetCassisLectures();
            //GetAllStudentsFromYale();
            //GetAllUniWithTransgenders();
            //GetAllLectureAtMDU();
            //UpdateCassi();
            DeleteNiklas();
        }

        public void InsertUnis()
        {
            dataContext.ExecuteCommand("delete from Universities");

            University yale = new University
            {
                Name = "Yale"
            };
            University mdu = new University
            {
                Name = "MDU"
            };

            dataContext.Universities.InsertOnSubmit(yale);
            dataContext.Universities.InsertOnSubmit(mdu);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudents()
        {

            University yale = dataContext.Universities.First(uni => uni.Name.Equals("Yale"));
            University mdu = dataContext.Universities.First(uni => uni.Name.Equals("MDU"));

            List<Student> students = new List<Student>
            {
                new Student { Name = "Cassandra", Gender = "Female", UniversityId = mdu.Id },
                new Student { Name = "Niklas", Gender = "transgender", University = yale },
                new Student { Name = "Juno", Gender = "Female", University = mdu },
                new Student { Name = "Lionel", Gender = "Male", University = yale },
                new Student { Name = "Matthew", Gender = "Male", University = mdu },
                new Student { Name = "Felicia", Gender = "Female", University = yale }
            };


            dataContext.Students.InsertAllOnSubmit(students);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLecture()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Programming" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });

            dataContext.SubmitChanges();

           MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student Cassandra = dataContext.Students.First(st => st.Name.Equals("Cassandra"));
            Student Niklas = dataContext.Students.First(st => st.Name.Equals("Niklas"));
            Student Juno = dataContext.Students.First(st => st.Name.Equals("Juno"));
            Student Lionel = dataContext.Students.First(st => st.Name.Equals("Lionel"));
            Student Matthew = dataContext.Students.First(st => st.Name.Equals("Matthew"));
            Student Felicia = dataContext.Students.First(st => st.Name.Equals("Felicia"));

            Lecture Programming = dataContext.Lectures.First(le => le.Name.Equals("Programming"));
            Lecture Math = dataContext.Lectures.First(le => le.Name.Equals("Math"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture{Student = Cassandra, Lecture = Programming });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Niklas, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Juno, Lecture = Programming });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Lionel, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Matthew, Lecture = Programming });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Felicia, Lecture = Math });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.StudentLectures;

        }

        public void GetUniOfCassi()
        {
            Student Cassandra = dataContext.Students.First(st => st.Name.Equals("Cassandra"));

            University CassisUni = Cassandra.University;

            List<University> unis = new List<University>
            {
                CassisUni,
            };

            MainDataGrid.ItemsSource = unis;
        }

        public void GetCassisLectures()
        {
            Student Cassandra = dataContext.Students.First(st => st.Name.Equals("Cassandra"));

            var CassisLecture = from sl in Cassandra.StudentLectures select sl.Lecture;

            
            MainDataGrid.ItemsSource = CassisLecture;
        }

        public void GetAllStudentsFromYale()
        {
            var students = from student in dataContext.Students where student.University.Name == "Yale" select student;

            MainDataGrid.ItemsSource = students; 
        }

        public void GetAllUniWithTransgenders()
        {
            var unis = from student in dataContext.Students join university in dataContext.Universities on student.University equals university where student.Gender == "transgender" select university;

            MainDataGrid.ItemsSource = unis;
        }

        public void GetAllLectureAtMDU()
        {
            var lectures = from studentLecture in dataContext.StudentLectures join student in dataContext.Students on studentLecture.StudentId equals student.Id where student.University.Name == "MDU" select studentLecture.Lecture;
            MainDataGrid.ItemsSource = lectures;
        }

        public void UpdateCassi()
        {
            Student Cassandra = dataContext.Students.FirstOrDefault(st => st.Name.Equals("Cassandra"));

            Cassandra.Name = "Cassi";

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteNiklas()
        {
            Student Niklas = dataContext.Students.FirstOrDefault(st => st.Name.Equals("Niklas"));

            dataContext.Students.DeleteOnSubmit(Niklas);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }
    }
}
