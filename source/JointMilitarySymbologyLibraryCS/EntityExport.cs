﻿/* Copyright 2014 Esri
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JointMilitarySymbologyLibrary
{
    public class EntityExport
    {
        // The super class for entity export objects.  This class holds
        // properties and methods that are used by child classes.

        protected ConfigHelper _configHelper;
        protected string _notes = "";

        //protected string[] _iconTypes = {"No Icon",
        //                                 "Center Icon",
        //                                 "Center + 1 Icon",
        //                                 "Center + 2 Icon",
        //                                 "Full Octagon Icon",
        //                                 "Full Frame Icon",
        //                                 "Special Icon"};

        protected string[] _iconTypes = {"No Icon",
                                         "Main Icon",
                                         "Main Icon",
                                         "Main Icon",
                                         "Main Icon",
                                         "Main Icon",
                                         "Main Icon"};

        protected string[] _geometryList = { "NotValid", "Point", "Line", "Area" };

        protected string BuildEntityCode(LibraryStandardIdentityGroup sig,
                                         SymbolSet ss,
                                         SymbolSetEntity e,
                                         SymbolSetEntityEntityType eType,
                                         SymbolSetEntityEntityTypeEntitySubType eSubType)
        {
            // Constructs a string containing the symbol set and entity codes for a given
            // set of those objects.

            string code = "";

            code = code + Convert.ToString(ss.SymbolSetCode.DigitOne) + Convert.ToString(ss.SymbolSetCode.DigitTwo);
            code = code + Convert.ToString(e.EntityCode.DigitOne) + Convert.ToString(e.EntityCode.DigitTwo);

            if (eType != null)
                code = code + Convert.ToString(eType.EntityTypeCode.DigitOne) + Convert.ToString(eType.EntityTypeCode.DigitTwo);
            else
                code = code + "00";

            if (eSubType != null)
                code = code + Convert.ToString(eSubType.EntitySubTypeCode.DigitOne) + Convert.ToString(eSubType.EntitySubTypeCode.DigitTwo);
            else
                code = code + "00";

            if (sig != null)
            {
                code = code + sig.GraphicSuffix;
            }

            return code;
        }

        protected string BuildEntityItemName(LibraryStandardIdentityGroup sig,
                                             SymbolSet ss,
                                             SymbolSetEntity e,
                                             SymbolSetEntityEntityType eType,
                                             SymbolSetEntityEntityTypeEntitySubType eSubType)
        {
            // Constructs a string containing the name of an entity, where each label value
            // is seperated by a DomainSeparator (usually a colon).  Builds this for each group
            // of related SymbolSet and entity.

            //string result = ss.Label.Replace(',', '-') + _configHelper.DomainSeparator + e.Label.Replace(',', '-');

            string result = (e.LabelAlias == "") ? e.Label : e.LabelAlias;

            if (eType != null)
            {
                string eTypeLabel = (eType.LabelAlias == "") ? eType.Label : eType.LabelAlias;
                result = result + _configHelper.DomainSeparator + eTypeLabel.Replace(',', '-');   
            }
            
            if (eSubType != null)
            {
                string eSubTypeLabel = (eSubType.LabelAlias == "") ? eSubType.Label : eSubType.LabelAlias;
                result = result + _configHelper.DomainSeparator + eSubTypeLabel.Replace(',', '-');
            }

            if (sig != null)
            {
                result = result + _configHelper.DomainSeparator + sig.Label;
            }
            
            return result;
        }

        protected string BuildEntityItemCategory(SymbolSet ss, IconType iconType)
        {
            // Contructs the category information for a given SymbolSet and entity, including the Label 
            // attribute of the SymbolSet and the type of icon being categorized, deperated by the
            // domain separator (usually a colon).

            string result =  ss.Label + _configHelper.DomainSeparator + _iconTypes[(int)iconType];
            
            return result;
        }

        protected string GrabGraphic(string uGraphic, string fGraphic, string nGraphic, string hGraphic, string gSuffix)
        {
            string graphic = "";

            switch (gSuffix)
            {
                case "_0":
                    graphic = uGraphic;
                    break;

                case "_1":
                    graphic = fGraphic;
                    break;

                case "_2":
                    graphic = nGraphic;
                    break;

                case "_3":
                    graphic = hGraphic;
                    break;
            }

            return graphic;
        }

        protected string BuildEntityItemTags(LibraryStandardIdentityGroup sig,
                                             SymbolSet ss,
                                             SymbolSetEntity e,
                                             SymbolSetEntityEntityType eType,
                                             SymbolSetEntityEntityTypeEntitySubType eSubType,
                                             bool omitSource)
        {
            // Constructs a string of semicolon delimited tags that users can utilize to search
            // for or find a given symbol.

            // The information concatenated together for this comes from a given SymbolSet and
            // entity (type and sub type).  Information includes the Label attributes, geometry
            // type, location of the original graphic file, the code, etc.

            string result = ss.Label.Replace(',', '-') + ";" + e.Label.Replace(',', '-');
            string graphic = "";
            string geometry = "";
            string iType = Convert.ToString(e.Icon);

            if (eType != null)
            {
                result = result + ";" + eType.Label.Replace(',', '-');
                iType = Convert.ToString(eType.Icon);
            }

            if (eSubType != null)
            {
                result = result + ";" + eSubType.Label.Replace(',', '-');
                iType = Convert.ToString(eSubType.Icon);

                // Add the type of geometry

                geometry = _geometryList[(int)eSubType.GeometryType];

                if (eSubType.Icon == IconType.FULL_FRAME)
                {
                    if (sig != null)
                    {
                        graphic = GrabGraphic(eSubType.CloverGraphic, eSubType.RectangleGraphic, eSubType.SquareGraphic, eSubType.DiamondGraphic, sig.GraphicSuffix);
                    }
                    
                    _notes = _notes + "icon touches frame;";
                }
                else if (eSubType.Icon == IconType.NA)
                    graphic = "";
                else
                    graphic = eSubType.Graphic;
            }
            else if(eType != null)
            {
                // Add the type of geometry

                geometry = _geometryList[(int)eType.GeometryType];

                if (eType.Icon == IconType.FULL_FRAME)
                {
                    if (sig != null)
                    {
                        graphic = GrabGraphic(eType.CloverGraphic, eType.RectangleGraphic, eType.SquareGraphic, eType.DiamondGraphic, sig.GraphicSuffix);
                    }
                    
                    _notes = _notes + "icon touches frame;";
                }
                else if (eType.Icon == IconType.NA)
                    graphic = "";
                else
                    graphic = eType.Graphic;
            }
            else if(e != null)
            {
                // Add the type of geometry

                geometry = _geometryList[(int)e.GeometryType];

                if (e.Icon == IconType.FULL_FRAME)
                {
                    if (sig != null)
                    {
                        graphic = GrabGraphic(e.CloverGraphic, e.RectangleGraphic, e.SquareGraphic, e.DiamondGraphic, sig.GraphicSuffix);
                    }
                    
                    _notes = _notes + "icon touches frame;";
                }
                else if (e.Icon == IconType.NA)
                    graphic = "";
                else
                    graphic = e.Graphic;
            }

            if (sig != null)
            {
                result = result + ";" + sig.Label;
            }

            result = result + ";" + iType;

            if(!omitSource)
                result = result + ";" + _configHelper.GetPath(ss.ID, FindEnum.FindEntities, true) + "\\" + graphic;

            result = result + ";" + geometry;
            result = result + ";" + BuildEntityItemName(sig, ss, e, eType, eSubType);
            result = result + ";" + BuildEntityCode(sig, ss, e, eType, eSubType);

            if (result.Length > 255)
            {
                // Can't have a tag string greater than 255 in length.
                // Human interaction will be required to resolve these on a case by case basis.

                _notes = _notes + "styleItemTags > 255;";
            }

            return result;
        }
    }
}
