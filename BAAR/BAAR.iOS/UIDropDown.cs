using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace BAAR.iOS
{

    public class UIDropDown
    {
        public List<UIButton> Items = new List<UIButton>();
        UIButton PrimaryButton;
        UIView RootView;
        
        public UIDropDown(UIView ViewToPlace)
        {
           // RootView = ViewToPlace.Window.RootViewController.View;
            PrimaryButton = new UIButton();
            PrimaryButton.BackgroundColor = UIColor.Green;
            PrimaryButton.Frame = new CoreGraphics.CGRect(25, 60, 40, 20);
            PrimaryButton.TouchUpInside += delegate
            {
                GenerateList();
            };

            ViewToPlace.AddSubview(PrimaryButton);
        }

        public void GenerateList()
        {
            for (int i = 0; i < 5; i++)
            {
                UIButton TestButton = new UIButton();
                TestButton.BackgroundColor = UIColor.Blue;
                TestButton.ClipsToBounds = true;
                TestButton.SetTitle("Classrooms", UIControlState.Normal);
                TestButton.Frame = new CoreGraphics.CGRect(25, PrimaryButton.Frame.Bottom + (i * 25), 250, 25);
                Items.Add(TestButton);
                TestButton.TouchUpInside += delegate {
                    Console.WriteLine("Hide!!");
                    for (int k = 0; k < Items.Count; k++)
                    {
                        Items[k].Hidden = true;
                    }
                   PrimaryButton.SetTitle(TestButton.Title(UIControlState.Normal),UIControlState.Normal);
                };

                RootView.AddSubview(TestButton);
            }


        }
    }
}