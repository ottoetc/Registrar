using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RegistrarApp
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void test_StudentEmptyAtFirst()
    {
      int result = Student.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverride()
    {
      DateTime testDate = new DateTime(2016, 01, 04);
      Student firstStudent = new Student("Nathan Otto", testDate);
      Student secondStudent = new Student("Nathan Otto", testDate);

      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save()
    {
      DateTime testDate = new DateTime(2016, 01, 04);
      Student testStudent = new Student("Nathan Otto", testDate);
      testStudent.Save();

      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_FindsStudent()
    {
        DateTime testDate = new DateTime(2016, 08, 08);
      //Arrange
      Student testStudent = new Student("Mow the lawn", testDate);
      testStudent.Save();

      //Act
      Student foundStudent = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, foundStudent);
    }


    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }


  }
}
