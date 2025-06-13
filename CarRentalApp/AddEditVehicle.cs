using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities _db;

        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            _db = new CarRentalEntities();
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            lblTitle.Text = "Edit Vehicle";
            isEditMode = true;
            _manageVehicleListing = manageVehicleListing;
            PopulateFields(carToEdit);
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLisencePlate.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbMake.Text) || string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }
                else
                {
                    //if(isEditMode == true)
                    if (isEditMode)
                    {
                        // Edit Code here
                        var id = int.Parse(lblId.Text);
                        var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLisencePlate.Text;

                        _db.SaveChanges();
                        Close();
                    }
                    else
                    {
                        // Add Code here
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = tbLisencePlate.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text)
                        };

                        _db.TypesOfCars.Add(newCar);

                    }

                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid(); // Refresh the vehicle listing grid
                    MessageBox.Show("Vehicle added successfully!");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

                private void btnCancel_Click(object sender, EventArgs e)
                {
                    Close();
                }
    }
}
