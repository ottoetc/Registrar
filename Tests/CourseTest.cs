using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RegistrarApp
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void test_CourseEmptyAtFirst()
    {
      int result = Course.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal()
    {
      Course firstCourse = new Course("HIST", 101);
      Course secondCourse = new Course("HIST", 101);

      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Test_SaveCourseToDatabase()
    {
      Course testCourse = new Course("HIST", 101);
      testCourse.Save();

      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCourseObject()
    {
      Course testCourse = new Course("HIST", 101);
      testCourse.Save();

      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Finds_Course()
    {
      Course testCourse = new Course("HIST", 101);
      testCourse.Save();

      Course foundCourse = Course.Find(testCourse.GetId());

      Assert.Equal(foundCourse, testCourse);
    }
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
    [Fact]
    public void Test_Delete_DeletesCourseAssociationsFromDatabase()
    {
      DateTime enrollmentDate = new DateTime(2016, 01, 04);
      Student testStudent = new Student("Nathan Otto", enrollmentDate)
      testStudent.Save();

      Course testCoursez  = new Course("HIST", 321);
      testCoursez.Save();

      testCourse.AddStudent(testStudent);
      testCourse.Delete();

      List<Course> resultStudentCourses = test.Student.GetCourses();
      List<Course> testStudentCourses = new List<Course> {};

      Assert.Equal(testStudentCourses, resultStudentCourses);
    }

  }
}
