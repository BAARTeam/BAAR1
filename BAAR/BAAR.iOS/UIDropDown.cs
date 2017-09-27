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
        public bool HasGenerated;
        public UIDropDown(UIView ViewToPlace,UIView MainView,List<string> Titles, CoreGraphics.CGRect Frame,UIScrollView Scroller,int TicketNumber)
        {
            Options = Titles;
            RootView = MainView;
            PrimaryButton = new UIButton();
            PrimaryButton.BackgroundColor = UIColor.DarkGray;
            PrimaryButton.SetTitle(Options[0],UIControlState.Normal);
            PrimaryButton.Frame = Frame;
            PrimaryButton.TouchUpInside += delegate
            {
                GenerateList(PrimaryButton,Scroller,TicketNumber);
            };

            ViewToPlace.AddSubview(PrimaryButton);
        }

        public void GenerateList(UIButton PrimButton,UIScrollView IsInScroll,int TicketOffset)
        {
            if (!HasGenerated)
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    UIButton TestButton = new UIButton();

                    TestButton.BackgroundColor = UIColor.Gray;
                    TestButton.ClipsToBounds = true;
                    TestButton.SetTitle(Options[i], UIControlState.Normal);

                    TestButton.Frame = new CoreGraphics.CGRect(25, (PrimButton.Frame.Y + (160 * TicketOffset) - IsInScroll.ContentOffset.Y) + (i * 25), 250, 25);
                    Items.Add(TestButton);
                    TestButton.TouchUpInside += delegate {
                        for (int k = 0; k < Items.Count; k++)
                        {
                            Items[k].Hidden = true;
                        }
                        PrimaryButton.SetTitle(TestButton.Title(UIControlState.Normal), UIControlState.Normal);
                    };
                    HasGenerated = true;
                    RootView.AddSubview(TestButton);
                }

            }
            else
            {

                for (int i = 0; i < Options.Count; i++)
                {
                    Items[i].Frame = new CoreGraphics.CGRect(25, (PrimButton.Frame.Y + (160 * TicketOffset) - IsInScroll.ContentOffset.Y) + (i * 25), 250, 25);
                }
                for (int k = 0; k < Items.Count; k++)
                {
                    Items[k].Hidden = false;
                }
            }

        }
    }
}