using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Projections;
using DotSpatial.Data;


namespace exercise5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lblmap2assumption_Click(object sender, EventArgs e)
        {

        }//误点

        private void button1_Click(object sender, EventArgs e)
        {
            //define the projections
            map1.Projection = KnownCoordinateSystems.Projected.UtmNad1983.NAD1983UTMZone12N;
            map2.Projection = KnownCoordinateSystems.Projected.NorthAmerica.NorthAmericaAlbersEqualAreaConic;
            map3.Projection = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousLambertConformalConic;
            map4.Projection = KnownCoordinateSystems.Projected.World.CylindricalEqualAreaworld;
            map5.Projection = KnownCoordinateSystems.Projected.Polar.NorthPoleAzimuthalEquidistant;
            map6.Projection = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousAlbersEqualAreaConicUSGS;

            //add the layers
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Shapefiles|*.shp";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                //add layer to first map
                FeatureSet featureSet1 = new FeatureSet();
                featureSet1.Open(fileDialog.FileName);

                //Populate the FiledName dropdownlist with the help of featureset1.
                //We need to pass featureset as an input paramter to FillColumnNames method.
                FillColumnNames(featureSet1);

                //set the projection
                featureSet1.Reproject(map1.Projection);
                map1.Layers.Add(featureSet1);

                //add layer to second map
                FeatureSet featureSet2 = new FeatureSet();
                featureSet2.Open(fileDialog.FileName);
                featureSet2.Reproject(map2.Projection);
                map2.Layers.Add(featureSet2);

                //add layer to map3
                FeatureSet featureSet3 = new FeatureSet();
                featureSet3.Open(fileDialog.FileName);
                featureSet3.Reproject(map3.Projection);
                map3.Layers.Add(featureSet3);

                //add layer to map4
                FeatureSet featureSet4 = new FeatureSet();
                featureSet4.Open(fileDialog.FileName);
                featureSet4.Reproject(map4.Projection);
                map4.Layers.Add(featureSet4);

                //add layer to map5
                FeatureSet featureSet5 = new FeatureSet();
                featureSet5.Open(fileDialog.FileName);
                featureSet5.Reproject(map5.Projection);
                map5.Layers.Add(featureSet5);

                //add layer to map6
                FeatureSet featureSet6 = new FeatureSet();
                featureSet6.Open(fileDialog.FileName);
                featureSet6.Reproject(map6.Projection);
                map6.Layers.Add(featureSet6);
            }

        }//误点

        private void btnGetTotalArea_Click(object sender, EventArgs e)
        {
            lbltotalAreaMap1.Text = "Total area in sq meters: " + _getTotalArea(map1).ToString();
            lbltotalAreaMap2.Text = "Total area in sq meters: " + _getTotalArea(map2).ToString();
            lbltotalAreaMap3.Text = "Total area in sq meters: " + _getTotalArea(map3).ToString();
            lbltotalAreaMap4.Text = "Total area in sq meters: " + _getTotalArea(map4).ToString();
            lbltotalAreaMap5.Text = "Total area in sq meters: " + _getTotalArea(map5).ToString();
            lbltotalAreaMap6.Text = "Total area in sq meters: " + _getTotalArea(map6).ToString();

        }
        /// <summary>
        /// This method is used to claculate total area of a feature
        /// </summary>
        /// <param name="mapInput">map control</param>
        /// <returns>total are of the feature from the mapcontrol</returns>
        /// <remarks></remarks>
        private double _getTotalArea(DotSpatial.Controls.Map mapInput)
        {
            double stateArea = 0;
            if ((mapInput.Layers.Count > 0))
            {
                MapPolygonLayer stateLayer = default(MapPolygonLayer);
                stateLayer = (MapPolygonLayer)mapInput.Layers[0];
                if ((stateLayer == null))
                {
                    MessageBox.Show("The layer is not a polygon layer.");
                }
                else
                {
                    foreach (IFeature stateFeature in stateLayer.DataSet.Features)
                    {
                        stateArea += stateFeature.Area();
                    }
                }
            }
            return stateArea;
        }
        /// <summary>
        /// This method is used to populate the FieldName combobox
        /// </summary>
        /// <param name="featureSet"></param>
        /// <remarks></remarks>
        private void FillColumnNames(IFeatureSet featureSet)
        {
            foreach (DataColumn column in featureSet.DataTable.Columns)
            {
                cmbFiledName.Items.Add(column.ColumnName);
            }
        }

        /// <summary>
        /// This method is used to fill the unique values on the cmbSelectedRegion combobox
        /// </summary>
        /// <param name="uniqueField">Fieldname combobox's selected value</param>
        /// <param name="mapInput">Map layer</param>
        /// <remarks></remarks>
        private void FillUniqueValues(string uniqueField, DotSpatial.Controls.Map mapInput)
        {
            List<string> fieldList = new List<string>();
            if ((mapInput.Layers.Count > 0))
            {
                MapPolygonLayer currentLayer = default(MapPolygonLayer);
                currentLayer = (MapPolygonLayer)mapInput.Layers[0];
                if ((currentLayer == null))
                {
                    MessageBox.Show("The layer is not a polygon layer.");
                }
                else
                {
                    DataTable dt = currentLayer.DataSet.DataTable;
                    cmbSelectedRegion.Items.Clear();
                    foreach (DataRow rows in dt.Rows)
                    {
                        cmbSelectedRegion.Items.Add(rows[uniqueField]);
                    }
                }
            }
        }

        private void cmbFiledName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillUniqueValues(cmbFiledName.Text, map1);
        }

        /// <summary>
        /// This sub method is used to control the visibility of any label control
        /// </summary>
        /// <param name="lbl">label name</param>
        /// <param name="vis">Either True / False</param>
        /// <remarks></remarks>
        private void setVisible(Label lbl, bool vis)
        {
            lbl.Visible = vis;
        }

        /// <summary>
        /// This method is used to get the area of the selected region on the combobox
        /// </summary>
        /// <param name="uniqueColumnName">Field name</param>
        /// <param name="uniqueValue">Unique value from the selected region combobox</param>
        /// <param name="mapInput">map layer</param>
        /// <returns>area of the selected field</returns>
        /// <remarks></remarks>
        private double _getArea(string uniqueColumnName, string uniqueValue, DotSpatial.Controls.Map mapInput)
        {
            double stateArea = 0;
            if ((mapInput.Layers.Count > 0))
            {
                MapPolygonLayer stateLayer = default(MapPolygonLayer);
                stateLayer = (MapPolygonLayer)mapInput.Layers[0];
                if ((stateLayer == null))
                {
                    MessageBox.Show("The layer is not a polygon layer.");
                }
                else
                {
                    stateLayer.SelectByAttribute("[" + uniqueColumnName + "] =" + "'" + uniqueValue + "'");
                    foreach (IFeature stateFeature in stateLayer.DataSet.Features)
                    {
                        if (uniqueValue.CompareTo(stateFeature.DataRow[uniqueColumnName]) == 0)
                        {
                            stateArea = stateFeature.Area();
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
            }
            return stateArea;
        }

        private void btnRegionArea_Click(object sender, EventArgs e)
        {
            setVisible(lblmap1selectedinfo, true);
            setVisible(lblmap2selectedinfo, true);
            setVisible(lblmap3selectedinfo, true);
            setVisible(lblmap4selectedinfo, true);
            setVisible(lblmap5selectedinfo, true);
            setVisible(lblmap6selectedinfo, true);
            lblMap1SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map1).ToString();
            lblMap2SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map2).ToString();
            lblMap3SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map3).ToString();
            lblMap4SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map4).ToString();
            lblMap5SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map5).ToString();
            lblMap6SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map6).ToString();

        }
        /// <summary>
        /// This function is used to claculate the difference between 2 ares
        /// </summary>
        /// <param name="area1"></param>
        /// <param name="area2"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private double _calculateDifference(double area1, double area2)
        {
            double areadiff = 0;
            areadiff = area1 - area2;
            return areadiff;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }//误点

        private void btnCompareProjections_Click(object sender, EventArgs e)
        {
            lblmap1difference.Text = _calculateDifference(Convert.ToDouble(lblMap2SelectedArea.Text), Convert.ToDouble(lblMap1SelectedArea.Text)).ToString();
            lblmap3difference.Text = _calculateDifference(Convert.ToDouble(lblMap2SelectedArea.Text), Convert.ToDouble(lblMap3SelectedArea.Text)).ToString();
            lblmap4difference.Text = _calculateDifference(Convert.ToDouble(lblMap2SelectedArea.Text), Convert.ToDouble(lblMap4SelectedArea.Text)).ToString();
            lblmap5difference.Text = _calculateDifference(Convert.ToDouble(lblMap2SelectedArea.Text), Convert.ToDouble(lblMap5SelectedArea.Text)).ToString();
            lblmap6difference.Text = _calculateDifference(Convert.ToDouble(lblMap2SelectedArea.Text), Convert.ToDouble(lblMap6SelectedArea.Text)).ToString();

            setVisible(lblmap1difference, true);
            setVisible(lblmap1info, true);
            setVisible(lblmap3difference, true);
            setVisible(lblmap3info, true);
            setVisible(lblmap4difference, true);
            setVisible(lblmap4info, true);
            setVisible(lblmap5difference, true);
            setVisible(lblmap5info, true);
            setVisible(lblmap6difference, true);
            setVisible(lblmap6info, true);
        }
    }
}
