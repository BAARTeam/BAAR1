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
        public List<string> Options = new List<string>();
        public UIDropDown(UIView ViewToPlace,UIView MainView,List<string> Titles, CoreGraphics.CGRect Frame)
        {
            Options = Titles;
            RootView = MainView;
            PrimaryButton = new UIButton();
            PrimaryButton.BackgroundColor = UIColor.Green;
            PrimaryButton.SetTitle(Options[0],UIControlState.Normal);
          //  PrimaryButton.Frame = new CoreGraphics.CGRect(25, 60, 40, 20);

           PrimaryButton.Frame = Frame;
            PrimaryButton.TouchUpInside += delegate
            {
                GenerateList(PrimaryButton);
            };

            ViewToPlace.AddSubview(PrimaryButton);
        }

        public void GenerateList(UIButton PrimButton)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                UIButton TestButton = new UIButton();
                TestButton.BackgroundColor = UIColor.Blue;
                TestButton.ClipsToBounds = true;
                TestButton.SetTitle(Options[i], UIControlState.Normal);
                TestButton.Frame = new CoreGraphics.CGRect(25, PrimButton.Frame.Bottom + (i * 25), 250, 25);
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