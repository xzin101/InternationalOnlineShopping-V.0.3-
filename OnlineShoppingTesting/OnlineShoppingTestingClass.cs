using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InternationalOnlineShopping;
using InternationalOnlineShopping.Controllers;
using InternationalOnlineShopping.Models;

namespace OnlineShoppingTesting
{
    [TestClass]
    public class OnlineShoppingTestingClass
    {
        [TestMethod]
        public void Delete()
        {
            // Arrange
            RegisterController controller = new RegisterController();

            // Act
            ViewResult result = controller.Delete(1) as ViewResult;


            // Assert
            Assert.IsInstanceOfType(result,typeof(Member));


        }
    }
}
