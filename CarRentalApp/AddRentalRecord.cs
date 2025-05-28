using CarRentalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CarRentalApp
{
    public partial class AddRentalRecord : Form
    {
        private readonly CarRentalEntities carRentalEntities;

        public AddRentalRecord()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtpRented.Value;
                var dateIn = dtpReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cmbCarType.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += ("Error: Please enter missing data\n\r");
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += ("Error: Illegal Date selection\n\r");
                }

                // if(isValid == true)
                if (isValid)
                {
                    var rentalRecord = new CarRentalRecord();
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarID = (int)cmbCarType.SelectedValue;

                    carRentalEntities.CarRentalRecords.Add(rentalRecord);
                    carRentalEntities.SaveChanges();

                    MessageBox.Show($"Thank you for renting, {customerName}\n\r" +
                    "\n\r" +
                    $"Car Rented: {carType}\n\r" +
                    $"Date Rented: {dateOut}\n\r" +
                    $"Date Returned: {dateIn}\n\r" +
                    $"Cost: {cost}\n\r" +
                    $"THANK YOU FOR YOUR BUSINESS!");
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; - this will end the program
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // select * from TypesofCards
            //var cars = carRentalEntities.TypesOfCars.ToList();           

            var cars = carRentalEntities.TypesOfCars.Select( q => new { Id = q.Id, Name = q.Make + " " + q.Model }).ToList();   
            cmbCarType.DisplayMember = "Name";
            cmbCarType.ValueMember = "Id";
            cmbCarType.DataSource = cars;
        }
    }
}
