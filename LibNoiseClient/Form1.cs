// 
// Copyright (c) 2013 Jason Bell
// 
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibNoise;
using LibNoise.Modifiers;

namespace LibNoiseClient
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        private Color[,] colors;
        private Bitmap earthLookupBitmap;
        private Color[] earthLookupTable;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = 0;

            bitmap = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            colors = new Color[pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height];

            earthLookupBitmap = new Bitmap(this.GetType().Assembly.GetManifestResourceStream("LibNoiseClient.EarthLookupTable.png"));
            earthLookupTable = new Color[earthLookupBitmap.Width];
            for (int i = 0; i < earthLookupBitmap.Width; i++)
                earthLookupTable[i] = earthLookupBitmap.GetPixel(i, 2);

            //pictureBox1.Image = bitmap;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Enabled = false;

            NoiseQuality quality = NoiseQuality.Standard;

            if (radiolow.Checked)
                quality = NoiseQuality.Low;
            if (radiostandard.Checked)
                quality = NoiseQuality.Standard;
            if (radiohigh.Checked)
                quality = NoiseQuality.High;

            int seed = 0;
            try
            {
                seed = Convert.ToInt32(textBox1.Text);
            }
            catch
            {
                seed = 0;
                textBox1.Text = "0";
            }

            int octaves = 0;
            try
            {
                octaves = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                octaves = 6;
                textBox2.Text = "6";
            }
            if (octaves > 30) octaves = 30;

            double frequency = 0;
            try
            {
                frequency = Convert.ToDouble(textBox3.Text);
            }
            catch
            {
                frequency = 0.05;
                textBox3.Text = "0.05";
            }

            double lacunarity = 0;
            try
            {
                lacunarity = Convert.ToDouble(textBox4.Text);
            }
            catch
            {
                lacunarity = 2.0;
                textBox4.Text = "2.0";
            }

            double persistence = 0;
            try
            {
                persistence = Convert.ToDouble(textBox5.Text);
            }
            catch
            {
                persistence = 0.5;
                textBox5.Text = "0.5";
            }

            bool mapToSphere = false;

            IModule module;
            switch (listBox1.SelectedItem.ToString())
            {
                case "Slow Perlin":
                    module = new Perlin();
                    ((Perlin)module).Frequency = frequency;
                    ((Perlin)module).NoiseQuality = quality;
                    ((Perlin)module).Seed = seed;
                    ((Perlin)module).OctaveCount = octaves;
                    ((Perlin)module).Lacunarity = lacunarity;
                    ((Perlin)module).Persistence = persistence;
                    break;
                case "Fast Perlin":
                    module = new FastNoise();
                    ((FastNoise)module).Frequency = frequency;
                    ((FastNoise)module).NoiseQuality = quality;
                    ((FastNoise)module).Seed = seed;
                    ((FastNoise)module).OctaveCount = octaves;
                    ((FastNoise)module).Lacunarity = lacunarity;
                    ((FastNoise)module).Persistence = persistence;
                    break;
                case "Slow Billow":
                    module = new Billow();
                    ((Billow)module).Frequency = frequency;
                    ((Billow)module).NoiseQuality = quality;
                    ((Billow)module).Seed = seed;
                    ((Billow)module).OctaveCount = octaves;
                    ((Billow)module).Lacunarity = lacunarity;
                    ((Billow)module).Persistence = persistence;
                    break;
                case "Fast Billow":
                    module = new FastBillow();
                    ((FastBillow)module).Frequency = frequency;
                    ((FastBillow)module).NoiseQuality = quality;
                    ((FastBillow)module).Seed = seed;
                    ((FastBillow)module).OctaveCount = octaves;
                    ((FastBillow)module).Lacunarity = lacunarity;
                    ((FastBillow)module).Persistence = persistence;
                    break;
                case "Slow Ridged Multifractal":
                    module = new RidgedMultifractal();
                    ((RidgedMultifractal)module).Frequency = frequency;
                    ((RidgedMultifractal)module).NoiseQuality = quality;
                    ((RidgedMultifractal)module).Seed = seed;
                    ((RidgedMultifractal)module).OctaveCount = octaves;
                    ((RidgedMultifractal)module).Lacunarity = lacunarity;
                    break;
                case "Fast Ridged Multifractal":
                    module = new FastRidgedMultifractal();
                    ((FastRidgedMultifractal)module).Frequency = frequency;
                    ((FastRidgedMultifractal)module).NoiseQuality = quality;
                    ((FastRidgedMultifractal)module).Seed = seed;
                    ((FastRidgedMultifractal)module).OctaveCount = octaves;
                    ((FastRidgedMultifractal)module).Lacunarity = lacunarity;
                    break;
                case "Slow Combined":
                    Billow billow = new Billow();
                    billow.Frequency = frequency;
                    billow.NoiseQuality = quality;
                    billow.Seed = seed;
                    billow.OctaveCount = octaves;
                    billow.Lacunarity = lacunarity;
                    billow.Persistence = persistence;

                    ScaleBiasOutput scaledBillow = new ScaleBiasOutput(billow);
                    scaledBillow.Bias = -0.75;
                    scaledBillow.Scale = 0.125;

                    RidgedMultifractal ridged = new RidgedMultifractal();
                    ridged.Frequency = frequency/2.0;
                    ridged.NoiseQuality = quality;
                    ridged.Seed = seed;
                    ridged.OctaveCount = octaves;
                    ridged.Lacunarity = lacunarity;

                    Perlin perlin = new Perlin();
                    perlin.Frequency = frequency/10.0;
                    perlin.NoiseQuality = quality;
                    perlin.Seed = seed;
                    perlin.OctaveCount = octaves;
                    perlin.Lacunarity = lacunarity;
                    perlin.Persistence = persistence;

                    Select selector = new Select(perlin, ridged, scaledBillow);
                    selector.SetBounds(0, 1000);
                    selector.EdgeFalloff = 0.5;

                    module = selector;
                    break;
                case "Fast Combined":
                    FastBillow fastbillow = new FastBillow();
                    fastbillow.Frequency = frequency;
                    fastbillow.NoiseQuality = quality;
                    fastbillow.Seed = seed;
                    fastbillow.OctaveCount = octaves;
                    fastbillow.Lacunarity = lacunarity;
                    fastbillow.Persistence = persistence;

                    ScaleBiasOutput fastscaledBillow = new ScaleBiasOutput(fastbillow);
                    fastscaledBillow.Bias = -0.75;
                    fastscaledBillow.Scale = 0.125;                    

                    FastRidgedMultifractal fastridged = new FastRidgedMultifractal();
                    fastridged.Frequency = frequency/2.0;
                    fastridged.NoiseQuality = quality;
                    fastridged.Seed = seed;
                    fastridged.OctaveCount = octaves;
                    fastridged.Lacunarity = lacunarity;

                    FastNoise fastperlin = new FastNoise();
                    fastperlin.Frequency = frequency/10.0;
                    fastperlin.NoiseQuality = quality;
                    fastperlin.Seed = seed;
                    fastperlin.OctaveCount = octaves;
                    fastperlin.Lacunarity = lacunarity;
                    fastperlin.Persistence = persistence;

                    Select fastselector = new Select(fastperlin, fastridged, fastscaledBillow);
                    fastselector.SetBounds(0, 1000);
                    fastselector.EdgeFalloff = 0.5;

                    module = fastselector;
                    break;
                case "Voronoi":
                    module = new Voronoi();
                    ((Voronoi)module).Frequency = frequency;
                    break;
                case "Slow Planet":
                    mapToSphere = true;

                    Perlin slowPlanetContinents = new Perlin();
                    slowPlanetContinents.Frequency = 1.5;

                    Billow slowPlanetLowlands = new Billow();
                    slowPlanetLowlands.Frequency = 4;
                    LibNoise.Modifiers.ScaleBiasOutput slowPlanetLowlandsScaled = new ScaleBiasOutput(slowPlanetLowlands);
                    slowPlanetLowlandsScaled.Scale = 0.2;
                    slowPlanetLowlandsScaled.Bias = 0.5;

                    RidgedMultifractal slowPlanetMountainsBase = new RidgedMultifractal();
                    slowPlanetMountainsBase.Frequency = 4;

                    ScaleBiasOutput slowPlanetMountainsScaled = new ScaleBiasOutput(slowPlanetMountainsBase);
                    slowPlanetMountainsScaled.Scale = 0.4;
                    slowPlanetMountainsScaled.Bias = 0.85;

                    FastTurbulence slowPlanetMountains = new FastTurbulence(slowPlanetMountainsScaled);
                    slowPlanetMountains.Power = 0.1;
                    slowPlanetMountains.Frequency = 50;

                    Perlin slowPlanetLandFilter = new Perlin();
                    slowPlanetLandFilter.Frequency = 6;

                    Select slowPlanetLand = new Select(slowPlanetLandFilter, slowPlanetLowlandsScaled, slowPlanetMountains);
                    slowPlanetLand.SetBounds(0, 1000);
                    slowPlanetLand.EdgeFalloff = 0.5;

                    Billow slowPlanetOceanBase = new Billow();
                    slowPlanetOceanBase.Frequency = 15;
                    ScaleOutput slowPlanetOcean = new ScaleOutput(slowPlanetOceanBase, 0.1);

                    Select slowPlanetFinal = new Select(slowPlanetContinents, slowPlanetOcean, slowPlanetLand);
                    slowPlanetFinal.SetBounds(0, 1000);
                    slowPlanetFinal.EdgeFalloff = 0.5;

                    module = slowPlanetFinal;
                    break;
                case "Fast Planet":
                    mapToSphere = true;

                    FastNoise fastPlanetContinents = new FastNoise(seed);
                    fastPlanetContinents.Frequency = 1.5;                    

                    FastBillow fastPlanetLowlands = new FastBillow();
                    fastPlanetLowlands.Frequency = 4;
                    LibNoise.Modifiers.ScaleBiasOutput fastPlanetLowlandsScaled = new ScaleBiasOutput(fastPlanetLowlands);
                    fastPlanetLowlandsScaled.Scale = 0.2;
                    fastPlanetLowlandsScaled.Bias = 0.5;

                    FastRidgedMultifractal fastPlanetMountainsBase = new FastRidgedMultifractal(seed);
                    fastPlanetMountainsBase.Frequency = 4;

                    ScaleBiasOutput fastPlanetMountainsScaled = new ScaleBiasOutput(fastPlanetMountainsBase);
                    fastPlanetMountainsScaled.Scale = 0.4;
                    fastPlanetMountainsScaled.Bias = 0.85;

                    FastTurbulence fastPlanetMountains = new FastTurbulence(fastPlanetMountainsScaled);
                    fastPlanetMountains.Power = 0.1;
                    fastPlanetMountains.Frequency = 50;

                    FastNoise fastPlanetLandFilter = new FastNoise(seed+1);
                    fastPlanetLandFilter.Frequency = 6;

                    Select fastPlanetLand = new Select(fastPlanetLandFilter, fastPlanetLowlandsScaled, fastPlanetMountains);
                    fastPlanetLand.SetBounds(0, 1000);
                    fastPlanetLand.EdgeFalloff = 0.5;

                    FastBillow fastPlanetOceanBase = new FastBillow(seed);
                    fastPlanetOceanBase.Frequency = 15;
                    ScaleOutput fastPlanetOcean = new ScaleOutput(fastPlanetOceanBase, 0.1);

                    Select fastPlanetFinal = new Select(fastPlanetContinents, fastPlanetOcean, fastPlanetLand);
                    fastPlanetFinal.SetBounds(0, 1000);
                    fastPlanetFinal.EdgeFalloff = 0.5;

                    module = fastPlanetFinal;
                    break;
                default:
                    module = new Constant(1.0);
                    break;
            }

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            LibNoise.Models.Sphere sphere = new LibNoise.Models.Sphere(module);

            for (int x = 0; x < pictureBox1.ClientSize.Width - 1; x++)
                for (int y = 0; y < pictureBox1.ClientSize.Height - 1; y++)
                {
                    double value;
                    if(mapToSphere)
                    {
                        int offsetX = -(x-512);
                        int offsetY = -(y-512);
                        double longitude = offsetY/5.6888888888;
                        if(longitude > 90.0) longitude = 90.0;
                        if(longitude < -90.0) longitude = -90.0;
                        double latitude = offsetX/2.844444444;
                        if(latitude > 180.0) latitude = 180.0;
                        if(latitude < -190.0) latitude = -180.0;
                        value = sphere.GetValue(longitude, latitude);
                    }
                    else
                        value = (module.GetValue(x, y, 10) + 1) / 2.0;
                    if (mapToSphere)
                    {
                        if (value < 0) value = 0;
                        if (value > 1.0) value = 1.0;
                        int index = (int)(value * earthLookupTable.Length);
                        if (index >= earthLookupTable.Length) index = earthLookupTable.Length - 1;
                        colors[x, y] = earthLookupTable[index];
                    }
                    else
                    {
                        if (value < 0) value = 0;
                        if (value > 1.0) value = 1.0;
                        byte intensity = (byte)(value * 255.0);
                        colors[x, y] = Color.FromArgb(255, intensity, intensity, intensity);
                    }
                }

            stopWatch.Stop();
            label2.Text = "Generation time for a 1024x1024 image, not including rendering: " + stopWatch.Elapsed.TotalMilliseconds + " ms";

            for (int x = 0; x < pictureBox1.ClientSize.Width - 1; x++)
                for (int y = 0; y < pictureBox1.ClientSize.Height - 1; y++)
                    bitmap.SetPixel(x, y, colors[x, y]);

            pictureBox1.Image = bitmap;

            panel2.Enabled = true;
        }
    }
}
