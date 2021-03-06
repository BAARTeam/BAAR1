﻿using System;
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
        public string Selected;
        StudentAccount StuAcc;
        public Func<int, string> OptionSelected;
        public UIButton PrimaryButton;
        public UIScrollView Scrolled;
        int TicketOffset;
        UIView RootView;
        public List<string> Options = new List<string>();
        public bool HasGenerated;
        public UIDropDown(UIView ViewToPlace, UIView MainView, List<string> Titles, CoreGraphics.CGRect Frame, UIScrollView Scroller, int TicketNumber, StudentAccount Main)
        {
            StuAcc = Main;
            Options = Titles;
            Selected = Options[0];
            RootView = MainView;
            Scrolled = Scroller;
            TicketOffset = TicketNumber;
            PrimaryButton = new UIButton();
            PrimaryButton.BackgroundColor = UIColor.FromRGBA(21, 21, 30, 255);
            PrimaryButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            PrimaryButton.SetTitle(Options[0], UIControlState.Normal);
            PrimaryButton.Frame = Frame;

            UIButton Arrow = new UIButton();
            Arrow.BackgroundColor = UIColor.FromRGBA(21, 21, 30, 255);
            Arrow.ContentMode = UIViewContentMode.ScaleAspectFit;
            Arrow.SetImage(UIImage.FromBundle("DropDownArrow.png"), UIControlState.Normal);
            Arrow.Frame = new CoreGraphics.CGRect(PrimaryButton.Frame.Right, PrimaryButton.Frame.Top, 25, Frame.Height);

            Arrow.TouchUpInside += delegate
            {
                GenerateList(PrimaryButton, Scroller, TicketNumber, false);
                StuAcc.OpenDropDown = this;
                Scrolled.UserInteractionEnabled = false;
            };
            PrimaryButton.TouchUpInside += delegate
            {
                GenerateList(PrimaryButton, Scroller, TicketNumber, false);
                StuAcc.OpenDropDown = this;
                Scrolled.UserInteractionEnabled = false;
            };

            ViewToPlace.AddSubview(PrimaryButton);
            ViewToPlace.AddSubview(Arrow);
        }
        UIButton PrimButtons;

        public void GenerateList(UIButton PrimButton, UIScrollView IsInScroll, int TicketOffset, bool AreOptionsChanged)
        {
            this.PrimButtons = PrimButton;

            if (AreOptionsChanged)
            {
                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Dispose();
            }
       //     if (!HasGenerated)
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    UIButton TestButton = new UIButton();

                    TestButton.BackgroundColor = UIColor.FromRGB(38, 37, 40);
                    TestButton.ClipsToBounds = true;
                    TestButton.SetTitle(Options[i], UIControlState.Normal);
                    TestButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

                    TestButton.Frame = new CoreGraphics.CGRect(25, (PrimButton.Frame.Y + (160 * TicketOffset) - IsInScroll.ContentOffset.Y) + (i * 45), 275, 45);
                    Items.Add(TestButton);
                    TestButton.TouchUpInside += delegate
                    {
                        if (OptionSelected != null)
                        {
                            Selected = TestButton.TitleLabel.Text.ToString();
                            OptionSelected(Options.IndexOf(TestButton.TitleLabel.Text));
                        }
                        for (int k = 0; k < Items.Count; k++)
                        {
                            Items[k].Hidden = true;
                        }
                        Scrolled.UserInteractionEnabled = true;
                        StuAcc.OpenDropDown = null;
                        PrimaryButton.SetTitle(TestButton.Title(UIControlState.Normal), UIControlState.Normal);
                    };
                    HasGenerated = true;
                    RootView.AddSubview(TestButton);
                }

            }
            //else
            //{

            //    for (int i = 0; i < Options.Count; i++)
            //    {
            //        Items[i].Frame = new CoreGraphics.CGRect(25, (PrimButton.Frame.Y + (160 * TicketOffset) - IsInScroll.ContentOffset.Y) + (i * 45), 250, 45);
            //    }
            //    for (int k = 0; k < Items.Count; k++)
            //    {
            //        Items[k].Hidden = false;
            //    }
            //}

        }

        public void GenerateNewOptions()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Dispose();
            }

            for (int i = 0; i < Options.Count; i++)
            {
                UIButton TestButton = new UIButton();

                TestButton.BackgroundColor = UIColor.FromRGB(38, 37, 40); ;
                TestButton.ClipsToBounds = true;
                TestButton.SetTitle(Options[i], UIControlState.Normal);
                TestButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

                TestButton.Frame = new CoreGraphics.CGRect(25, (PrimaryButton.Frame.Y + (160 * TicketOffset) - Scrolled.ContentOffset.Y) + (i * 45), 275, 45);
                Items.Add(TestButton);
                TestButton.TouchUpInside += delegate
                {
                    if (OptionSelected != null)
                    {
                        Selected = TestButton.TitleLabel.Text.ToString();
                        Console.WriteLine("This Thing  " + Selected);
                        OptionSelected(Options.IndexOf(TestButton.TitleLabel.Text));
                    }
                    for (int k = 0; k < Items.Count; k++)
                    {
                        Items[k].Hidden = true;
                    }
                    Scrolled.UserInteractionEnabled = true;
                    StuAcc.OpenDropDown = null;
                    PrimaryButton.SetTitle(TestButton.Title(UIControlState.Normal), UIControlState.Normal);
                };
                HasGenerated = true;
                RootView.AddSubview(TestButton);
                PrimaryButton.SetTitle(Options[0], UIControlState.Normal);
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Hidden = true;
            }
        }
    }
}
        