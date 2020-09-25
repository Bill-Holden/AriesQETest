using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AriesQETest
{
    public class TestMyStore
    {
        string summerDressUrl = "http://automationpractice.com/index.php?id_category=11&controller=category";
        string loginUrl = "http://automationpractice.com/index.php?controller=authentication&back=my-account";
        IWebDriver driver;

        [SetUp]
        public void OpenSite()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            
            // set implicit wait to 20 seconds and open the site
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(summerDressUrl);

            // confirm that I'm on the right page
            IWebElement summerDressImage = driver.FindElement(By.ClassName("category-name"));
            Assert.AreEqual("Summer Dresses", summerDressImage.Text);
        }

        [Test]
        public void TestDressAdd()
        {
            // obtain initial cart count so we can verify +1             
            int initialCartCount;           

            try // if cart is empty, trying to get the count will throw
            {
                IWebElement initialCart = driver.FindElement(By.XPath("//*[@id=\"header\"]/div[3]/div/div/div[3]/div/a/span[1]"));
                initialCartCount = Int32.Parse(initialCart.Text);
            }

            catch (Exception)
            {
                // if it doesn't contain a number, set it to 0
                initialCartCount = 0;
            }

            // add a dress to the cart
            IWebElement dressContainer = driver.FindElement(By.XPath("//*[@id=\"center_column\"]/ul"));            
            IWebElement firstDress = dressContainer.FindElement(By.ClassName("product-container"));
            firstDress.Click();

            IWebElement addToCart = driver.FindElement(By.Id("add_to_cart"));
            addToCart.Click();


            //close the popup           
            IWebElement closePopup = driver.FindElement(By.XPath("//*[@id=\"layer_cart\"]/div[1]/div[1]/span"));
            closePopup.Click();

            // confirm that it's in the cart while taking into consideration I may already have had something in my cart
            IWebElement updatedCart = driver.FindElement(By.XPath("//*[@id=\"header\"]/div[3]/div/div/div[3]/div/a/span[1]"));
            int updatedCartCount = Int32.Parse(updatedCart.Text);
            Assert.AreEqual(initialCartCount + 1, updatedCartCount);

            // Go to the sign in page and confirm you navigated there
            IWebElement loginLink = driver.FindElement(By.XPath("//*[@id=\"header\"]/div[2]/div/div/nav/div[1]/a"));
            loginLink.Click();
            Assert.AreEqual(loginUrl,driver.Url);
        }

        [TearDown]
        public void Cleanup()
        {          
            driver.Quit();
        }


    }
}
