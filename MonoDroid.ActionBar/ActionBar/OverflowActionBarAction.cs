/*
 * Copyright (C) 2012 James Montemagno <http://www.montemagno.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
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
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.ActionBarSample
{
    class OverflowActionBarAction : ActionBarAction, AdapterView.IOnItemSelectedListener
    {
        public int MenuItemId;
        private readonly List<string> m_StringIds;
        private Spinner m_OverflowSpinner;
        public Activity Activity { get; set; }
        public int Index { get; set; }
        public OverflowActionBarAction(Context context)
        {
            mDrawable = Resource.Drawable.ic_action_overflow_dark;
            mContext = context;
            ActionList = new List<ActionBarAction>();
            m_StringIds = new List<string> {string.Empty/*first one to hide*/};
            ActionType = ActionType.Always;
        }

        public List<ActionBarAction> ActionList { get; set; }

        public Spinner OverflowSpinner
        {
            get { return m_OverflowSpinner; }
            set 
            { 
                m_OverflowSpinner = value;
                m_OverflowSpinner.OnItemSelectedListener = this;
            }
        }

        public void ClearActions()
        {
            ActionList.Clear();
            m_StringIds.Clear();
            m_StringIds.Add(string.Empty);//add back in first one cause we have to.
        }

        public override int GetDrawable()
        {
            return mDrawable;
        }

        public void AddAction(ActionBarAction actionBarAction)
        {
            ActionList.Add(actionBarAction);
            m_StringIds.Add(actionBarAction.PopUpMessage == 0 ? "ERROR" : mContext.Resources.GetString(actionBarAction.PopUpMessage));
        }

        private bool m_FirstClick;
        public override void PerformAction(View view)
        {
            try
            {
                if(m_OverflowSpinner == null)
                    return;

                m_OverflowSpinner.Adapter = new OverflowSpinnerAdapter(Activity, m_StringIds);
                m_FirstClick = true;
                m_OverflowSpinner.SetSelection(0);
                m_OverflowSpinner.PerformClick();
                
            }
            catch (Exception ex)
            {
            }
        }

        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            if(m_FirstClick)
            {
                m_FirstClick = false;
                return;
            }

            ActionList[position-1].PerformAction(view);//subtract 1 for default.
        }

        public void OnNothingSelected(AdapterView parent)
        {
            //nothing to see here
        }
    }

    class OverflowSpinnerAdapter : BaseAdapter
    {
        private readonly Activity m_Context;
        private readonly IEnumerable<string> m_Items;



        public OverflowSpinnerAdapter(Activity context, IEnumerable<string> items)
        {
            m_Context = context;
            m_Items = items;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (position < 0)
                return null;

            View view;
            var item = m_Items.ElementAt(position);
            if (!string.IsNullOrEmpty(item))
                view = m_Context.LayoutInflater.Inflate(Resource.Layout.SpinnerItem, parent, false);
            else
            {
                view = m_Context.LayoutInflater.Inflate(Resource.Layout.BlankSpinner, parent, false);//hack to get first item blank.
                return view;
            }


            if (view == null)
                return null;


            var itemView = view.FindViewById<CheckedTextView>(Android.Resource.Id.Text1);
            itemView.Text = item;

            return view;
        }

        public override int Count
        {
            get { return m_Items.Count(); }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

    }

}