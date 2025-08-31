using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ckir_crasher_2.Classes
{
    internal class BorderController
    {

        public static void Controll_with_animation(ref Border targetborder, int midifi, DependencyProperty dp)
        {
            DoubleAnimation animation = new DoubleAnimation();
            QuadraticEase quad = new QuadraticEase();

            if (midifi == 0)
            {
                animation.From = targetborder.Width;
                animation.To = 0;
                quad.EasingMode = EasingMode.EaseIn;
                animation.EasingFunction = quad;


            }
            else
            {
                animation.From = 0;
                animation.To = midifi;
                quad.EasingMode |= EasingMode.EaseOut;
                animation.EasingFunction = quad;
            }

            animation.Duration = TimeSpan.FromSeconds(0.35);
            targetborder.BeginAnimation(dp, animation);
        }

        public static void Controll_with_animation_v(ref Border targetborder, int midifi, DependencyProperty dp)
        {
            DoubleAnimation animation = new DoubleAnimation();
            QuadraticEase quad = new QuadraticEase();

            if (midifi == 0)
            {
                animation.From = targetborder.Height;
                animation.To = 0;
                quad.EasingMode = EasingMode.EaseIn;
                animation.EasingFunction = quad;


            }
            else
            {
                animation.From = 0;
                animation.To = midifi;
                quad.EasingMode |= EasingMode.EaseOut;
                animation.EasingFunction = quad;
            }

            animation.Duration = TimeSpan.FromSeconds(0.25);
            targetborder.BeginAnimation(dp, animation);
        }
    }
}
