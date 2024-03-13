using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
namespace FuzzPhyte.Control.Editor
{
    //Assets/Samples/Product_Name_Unity/Version/PackageSample Location e.g. 'FP Control Samples'
    [Serializable]
    public static class FPControlUtility 
    {
        public const string PRODUCT_NAME = "FP_Control";
        public const string PRODUCT_NAME_UNITY = "FP Control";
        public const string BASEVERSION = "0.5.0";

        public const string SAMPLELOCALFOLDER = PRODUCT_NAME_UNITY + " Samples";
        public const string SAMPLELOCALURP = PRODUCT_NAME_UNITY + " Samples URP";

        public const string SAMPLESPATH = "Assets/" + PRODUCT_NAME + "/Samples/URPSamples";
        public const string INSTALLSAMPLEPATH = "Samples\\" + PRODUCT_NAME_UNITY + "\\";

        public const string CAT0 = "ControlParameters";

        public static string ReturnInstallPath()
        {
            //var localpath = INSTALLSAMPLEPATH + BASEVERSION;
            
            return INSTALLSAMPLEPATH + BASEVERSION;
        }

    }
}
