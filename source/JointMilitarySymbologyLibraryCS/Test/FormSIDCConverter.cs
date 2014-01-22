﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using JointMilitarySymbologyLibrary;

namespace Test
{
    public partial class FormSIDCConverter : Form
    {
        private Librarian _librarian;
        private Symbol _symbol;

        public FormSIDCConverter()
        {
            InitializeComponent();

            _librarian = new Librarian();
            _symbol = _librarian.MakeSymbol(new SIDC());

            updateControls();
        }

        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }

        private void buttonCtoD_Click(object sender, EventArgs e)
        {
            _symbol.LegacySIDC = text2525C.Text;

            updateD();
            updateImage();
        }

        private void buttonDtoC_Click(object sender, EventArgs e)
        {
            SIDC sidc = _symbol.SIDC;

            sidc.PartAString = text2525D_1.Text;
            sidc.PartBString = text2525D_2.Text;

            _symbol.SIDC = sidc;

            updateC();
            updateImage();
        }

        // Update controls on the form

        private void updateC()
        {
            text2525C.Text = _symbol.LegacySIDC;
        }

        private void updateD()
        {
            text2525D_1.Text = _symbol.SIDC.PartAString;
            text2525D_2.Text = _symbol.SIDC.PartBString;
        }

        private void updateControls()
        {
            updateC();
            updateD();
            updateImage();
        }

        private void updateImage()
        {
            if (_symbol.Image != null)
            {
                pictureBoxSymbol.Image = _symbol.Image;

                _symbol.SaveImage("C:\\Users\\andy750\\Documents\\jmsml\\save.png");
            }
        }
    }
}