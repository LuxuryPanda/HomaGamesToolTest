/***
 *
 * @Author: Roman
 * @Created on: 11/02/23
 *
 ***/

using System;

namespace _3_Scripts.Editor.Helpers
{
    public class EditorTab
    {
        #region ## Fields ##

        public string Name;
        
        public Action OnTabSelected;

        #endregion

        #region ## Constructor ##
        
        public EditorTab(string name, Action onTabSelected)
        {
            Name = name;
            OnTabSelected = onTabSelected;
        }

        #endregion
    }
}