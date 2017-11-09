﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateCalculationSystem.Core.Calculations;
using RateCalculationSystem.Core.Calculations.RateCalculation;
using RateCalculationSystem.Core.Models.Rate;

namespace RateCalculationSystem.Core.Test
{
    [TestClass]
    public class RateCalculationTest
    {
        /// <summary>
        ///     Expect exception when argument equals null
        /// </summary>
        [TestMethod]
        public void expect_exception_when_argument_equals_null()
        {
            ICalculation<LenderOfferInputModel<decimal>, RateOutputModel<decimal>> rateCalculation =
                new RateCalculation();

            ExpectException<ArgumentNullException>(() => rateCalculation.Calculate(null));
        }


        /// <summary>
        ///     Except exception when arguments equal zero
        /// </summary>
        [TestMethod]
        public void expect_exception_when_arguments_equal_zero()
        {
            ICalculation<LenderOfferInputModel<decimal>, RateOutputModel<decimal>> rateCalculation =
                new RateCalculation();

            var loanAmount = 1000;
            var rate = 0.07;
            var term = 36;

            // except exception because of amount equals zero
            ExpectException<ArgumentException>(() =>
                rateCalculation.Calculate(new LenderOfferInputModel<decimal>(0, term, rate)));

            // except exception because of term equals zero
            ExpectException<ArgumentException>(() =>
                rateCalculation.Calculate(new LenderOfferInputModel<decimal>(loanAmount, 0, rate)));

            // except exception because of rate equals zero
            ExpectException<ArgumentException>(() =>
                rateCalculation.Calculate(new LenderOfferInputModel<decimal>(loanAmount, term, 0)));
        }

        /// <summary>
        ///     Check requested amount
        /// </summary>
        [TestMethod]
        public void expect_true_for_correct_requested_amount()
        {
            ICalculation<LenderOfferInputModel<decimal>, RateOutputModel<decimal>> rateCalculation =
                new RateCalculation();

            var loanAmount = 1000;
            var rate = 0.07;
            var term = 36;
            var result = rateCalculation.Calculate(new LenderOfferInputModel<decimal>(loanAmount, term, rate));

            Assert.AreEqual(loanAmount, result.RequstedAmount);
        }


        /// <summary>
        ///     Check monthly repayment
        /// </summary>
        [TestMethod]
        public void expect_true_for_correct_monthly_repayment()
        {
            ICalculation<LenderOfferInputModel<decimal>, RateOutputModel<decimal>> rateCalculation =
                new RateCalculation();

            var loanAmount = 1000;
            var rate = 0.07;
            var term = 36;
            var result = rateCalculation.Calculate(new LenderOfferInputModel<decimal>(loanAmount, term, rate));

            // excepted result is 30.87 
            var expectedMonthlyPayment = (decimal) 30.88;
            var currentMonthlyPayment = Math.Truncate(result.MonthlyPayment * 100) / 100;
            Assert.AreEqual(expectedMonthlyPayment, currentMonthlyPayment);
        }


        /// <summary>
        ///     check total repayment
        /// </summary>
        [TestMethod]
        public void expect_true_for_correct_total_repayment()
        {
            ICalculation<LenderOfferInputModel<decimal>, RateOutputModel<decimal>> rateCalculation =
                new RateCalculation();

            var loanAmount = 1000;
            var rate = 0.07;
            var term = 36;
            var result = rateCalculation.Calculate(new LenderOfferInputModel<decimal>(loanAmount, term, rate));

            // excepted result is true
            var expectedTotalPayment = (decimal) 1111.68;
            var currentTotalPayment = Math.Truncate(result.TotalPayment * 100) / 100;
            Assert.AreEqual(expectedTotalPayment, currentTotalPayment);
        }

       


        /// <summary>
        ///     Generic function for exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public static void ExpectException<T>(Action action) where T : Exception
        {
            try
            {
                action();
                Assert.Fail("Expected exception " + typeof(T).Name);
            }
            catch (T exception)
            {
                Assert.AreEqual(typeof(T).Name, exception.GetType().Name);
            }
        }
    }
}