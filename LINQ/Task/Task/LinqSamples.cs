// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Where - Task 1")]
        [Description("This sample return list of customers with total orders sum greater than x.")]

        public void Linq1()
        {
            decimal x = 26656;

            var list = dataSource.Customers
                .Where(c => c.Orders.Sum(o => o.Total) > x)
                .Select(customer => new
                {
                    customerId = customer.CustomerID,
                    totalOrderSum = customer.Orders.Select(order => order.Total).Sum()
                });

            ObjectDumper.Write($"Customers with total order sum greater than {x}:");
            ObjectDumper.Write("________________________");
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.customerId}. Total order sum: {customer.totalOrderSum}");
            }

            x = 100000;

            ObjectDumper.Write($"Customers with total order sum greater than {x}:");
            ObjectDumper.Write("________________________");
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.customerId}. Total order sum: {customer.totalOrderSum}");
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 2")]
        [Description("This sample return list of customers and suppliers from the same country and city.")]

        public void Linq2()
        {
            //With grouping
            var list = dataSource.Customers
                 .GroupJoin(dataSource.Suppliers,
                     c => new { c.City, c.Country },
                     s => new { s.City, s.Country },
                     (c, s) => new 
                     {
                         Customer = c,
                         Suppliers = s.Select(x => x.SupplierName)
                     })
                 .Where(x => x.Suppliers.Any());

            ObjectDumper.Write("List of customers and suppliers from the same country and city with grouping:");
            ObjectDumper.Write("________________________");
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.Customer.CustomerID}. " +
                    $"Suppliers from the same country and city: {string.Join(", ", customer.Suppliers)}");
            }

            //Without grouping
            var anotherList = dataSource.Customers
                 .Select(c => new
                 {
                     Customer = c,
                     SupplierNames = dataSource.Suppliers.Where(s => s.City == c.City && s.Country == c.Country)
                     .Select(s => s.SupplierName)
                 })
                 .Where(x => x.SupplierNames.Any());

            ObjectDumper.Write("________________________");
            ObjectDumper.Write("List of customers and suppliers from the same country and city without grouping:");
            ObjectDumper.Write("________________________");
            foreach (var customer in anotherList)
            {
                ObjectDumper.Write($"Customer: {customer.Customer.CustomerID}. " +
                    $"Suppliers from the same country and city: {string.Join(", ", customer.SupplierNames)}");
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 3")]
        [Description("This sample return list of customers who have order sum greater than x.")]
        public void Linq3()
        {
            decimal x = 8000;

            var list = dataSource.Customers
                .Where(c => c.Orders.Any(o => o.Total > x));

            ObjectDumper.Write($"List of customers who have order sum greater than {x}:");
            ObjectDumper.Write("________________________"); 
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.CustomerID}. List of orders:");
                foreach (var order in customer.Orders)
                {
                    ObjectDumper.Write($"{order.Total}");
                }
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 4")]
        [Description("This sample return list of customers with first order date.")]
        public void Linq4()
        {
            var list = dataSource.Customers
                .Where(x => x.Orders.Any())
                .Select(c => new
                {
                    CustomerId = c.CustomerID,
                    OrderDate = c.Orders.Select(o => o.OrderDate).First()
                });

            ObjectDumper.Write($"List of customers with first order date:");
            ObjectDumper.Write("________________________"); 
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.CustomerId}. First order date: {customer.OrderDate}");
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 5")]
        [Description("This sample returns list of customers with first order date sorted by year, month, total order sum and customer name.")]
        public void Linq5()
        {
            var list = dataSource.Customers
                .Where(x => x.Orders.Any())
                .Select(c => new
                {
                    CustomerId = c.CustomerID,
                    OrderDate = c.Orders.Select(o => o.OrderDate).First(),
                    TotalOrderSum = c.Orders.Sum(o => o.Total)
                })
                .OrderBy(c => c.OrderDate.Year)
                .ThenBy(c => c.OrderDate.Month)
                .ThenByDescending(c => c.TotalOrderSum)
                .ThenBy(c => c.CustomerId);

            ObjectDumper.Write($"List of customers with first order date sorted by year, month, total order sum and customer name:");
            ObjectDumper.Write("________________________");
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.CustomerId}. First order date year: {customer.OrderDate}; total orders sum: {customer.TotalOrderSum}.");
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 6")]
        [Description("This sample return list of customers with nondigital postal code, empty region or without code operator.")]
        public void Linq6()
        {
            var list = dataSource.Customers
                .Where(c => !int.TryParse(c.PostalCode, out var res)
                          || string.IsNullOrWhiteSpace(c.Region)
                          || !c.Phone.StartsWith("(", StringComparison.Ordinal));

            ObjectDumper.Write($"list of customers with nondigital postal code, empty region or without code operator:");
            ObjectDumper.Write("________________________");
            foreach (var customer in list)
            {
                ObjectDumper.Write($"Customer: {customer.CustomerID}. Postal code: {customer.PostalCode}. Region: {customer.Region}. Phone: {customer.Phone}.");
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 7")]
        [Description("This sample return list of products grouped by category and by stock availability and ordered by price.")]
        public void Linq7()
        {
            var list = dataSource.Products
                .GroupBy(p => p.Category)
                .Select(x => new
                {
                    Category = x.Key,
                    Availability = x.GroupBy(c => c.UnitsInStock > 0)
                        .Select(c => new
                        {
                            Availability = c.Key,
                            Products = c.OrderBy(p => p.UnitPrice)
                        })
                });

            ObjectDumper.Write($"List of products grouped by category and by stock availability and ordered by price:");
            foreach (var product in list)
            {
                ObjectDumper.Write("________________________");
                ObjectDumper.Write($"Product category: {product.Category}.");
                foreach (var prod in product.Availability)
                {
                    ObjectDumper.Write($"Product availability: {prod.Availability}.");
                    ObjectDumper.Write("________________________");
                    foreach (var p in prod.Products)
                    {
                        ObjectDumper.Write($"Product price: {p.UnitPrice}.");
                    }
                }
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 8")]
        [Description("This sample return list of products grouped by price.")]
        public void Linq8()
        {
            int cheapPrice = 30, expensivePrice = 50;

            var list = dataSource.Products
                .GroupBy(p => p.UnitPrice < cheapPrice ? "cheap" 
                    : p.UnitPrice < expensivePrice ? "average" : "expensive");

            ObjectDumper.Write($"List of products grouped by price:");
            ObjectDumper.Write("________________________");
            foreach (var group in list)
            {
                ObjectDumper.Write($"Group name: {group.Key}.");
                foreach (var product in group)
                {
                    ObjectDumper.Write($"Product price: {product.UnitPrice}.");
                }
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 9")]
        [Description("This sample return average income and intensity of city.")]
        public void Linq9()
        {
            var list = dataSource.Customers
                .Where(c => c.Orders.Any())
                .GroupBy(c => c.City)
                .Select(x => new
                {
                    City = x.Key,
                    Income = x.Average(o => o.Orders.Average(p => p.Total)),
                    Intensity = x.Average((o => o.Orders.Count()))
                });

            ObjectDumper.Write($"List of average income and intensity of city:");
            ObjectDumper.Write("________________________");
            foreach (var city in list)
            {
                ObjectDumper.Write($"City: {city.City}. Average Income: {city.Income}. Average Intensity: {city.Intensity}.");
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 10")]
        [Description("This sample return client statistics activity by Month, by Year, by Month and Year.")]
        public void Linq10()
        {
            var statistics = dataSource.Customers
                .Where(c => c.Orders.Any())
                .Select(c => new
                {
                    CustomerID = c.CustomerID,
                    YearStatistics = c.Orders
                        .GroupBy(x => x.OrderDate.Year)
                        .Select(g => new
                        {
                            Year = g.Key,
                            OrdersCount = g.Count()
                        }),
                    MonthStatistics = c.Orders
                        .GroupBy(x => x.OrderDate.Month)
                        .Select(g => new
                        {
                            Month = g.Key,
                            OrdersCount = g.Count()
                        }),
                    MonthAndYearStatistics = c.Orders
                        .GroupBy(x => new { x.OrderDate.Month, x.OrderDate.Year })
                        .Select(g => new
                        {
                            Month = g.Key.Month,
                            Year = g.Key.Year,
                            OrdersCount = g.Count()
                        })
                    });

            ObjectDumper.Write($"Client statistics activity:");
            foreach (var statistic in statistics)
            {
                ObjectDumper.Write($"Client statistics activity by year.");
                ObjectDumper.Write("________________________");
                foreach (var year in statistic.YearStatistics)
                {
                    ObjectDumper.Write($"Year: {year.Year}. Count of orders: {year.OrdersCount}.");
                }

                ObjectDumper.Write($"Client statistics activity by month.");
                ObjectDumper.Write("________________________");
                foreach (var month in statistic.MonthStatistics)
                {
                    ObjectDumper.Write($"Month: {month.Month}. Count of orders: {month.OrdersCount}.");
                }

                ObjectDumper.Write($"Client statistics activity by month and year.");
                ObjectDumper.Write("________________________");
                foreach (var monthAndYear in statistic.MonthAndYearStatistics)
                {
                    ObjectDumper.Write($"Year: {monthAndYear.Year}. Month: {monthAndYear.Month}. Count of orders: {monthAndYear.OrdersCount}.");
                }
            }
        }
    }
}
