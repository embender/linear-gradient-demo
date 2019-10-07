using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace linear_gradient_sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }
        private Compositor _compositor;

        private SpriteVisual _vis1;
        private SpriteVisual _vis2;
        private SpriteVisual _vis3;
        private SpriteVisual _vis4;

        private CompositionLinearGradientBrush _linearGradientBrush;
        private CompositionColorGradientStop _gradientStop1;
        private CompositionColorGradientStop _gradientStop2;

        private Color warmColor1 = Colors.DeepPink;
        private Color warmColor2 = Colors.Honeydew;
        private Color coolColor1 = Colors.LightSkyBlue;
        private Color coolColor2 = Colors.Teal;

        private Vector3KeyFrameAnimation _scaleAnim;
        private ScalarKeyFrameAnimation _offsetAnim;

        private ColorKeyFrameAnimation _colorAnimGradientStop1;
        private ColorKeyFrameAnimation _colorAnimGradientStop2;
        private ColorKeyFrameAnimation _changeb1stop1;
        private ColorKeyFrameAnimation _changeb1stop2;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            // Update Rectangle widths to match text width
            Rectangle1.Width = TextBlock1.ActualWidth;
            Rectangle2.Width = TextBlock2.ActualWidth;
            Rectangle3.Width = TextBlock3.ActualWidth;
            Rectangle4.Width = TextBlock4.ActualWidth;

            // Create the four visuals that will be used to hold the LinearGradient brush
            _vis1 = _compositor.CreateSpriteVisual();
            _vis2 = _compositor.CreateSpriteVisual();
            _vis3 = _compositor.CreateSpriteVisual();
            _vis4 = _compositor.CreateSpriteVisual();

            // Create the linearGradient brush and set the first set of colors to the gradientStops of the Brush
            _linearGradientBrush = _compositor.CreateLinearGradientBrush();
            _gradientStop1 = _compositor.CreateColorGradientStop();
            _gradientStop1.Offset = 0;
            _gradientStop1.Color = warmColor1;
            _gradientStop2 = _compositor.CreateColorGradientStop();
            _gradientStop2.Offset = 1;
            _gradientStop2.Color = warmColor2;
            _linearGradientBrush.ColorStops.Add(_gradientStop1);
            _linearGradientBrush.ColorStops.Add(_gradientStop2);

            // Paint visuals with brushes and set their locations
            _vis1.Brush = _linearGradientBrush;
            _vis1.Scale = new Vector3(0, 1, 0);
            _vis1.Size = new Vector2((float)Rectangle1.ActualWidth, (float)Rectangle1.ActualHeight);
       
            _vis2.Brush = _linearGradientBrush;
            _vis2.Scale = new Vector3(0, 1, 0);
            _vis2.Size = new Vector2((float)Rectangle2.ActualWidth, (float)Rectangle2.ActualHeight);
            
            _vis3.Brush = _linearGradientBrush;
            _vis3.Scale = new Vector3(0, 1, 0);
            _vis3.Size = new Vector2((float)Rectangle3.ActualWidth, (float)Rectangle3.ActualHeight);
            
            _vis4.Brush = _linearGradientBrush;
            _vis4.Scale = new Vector3(0, 1, 0);
            _vis4.Size = new Vector2((float)Rectangle4.ActualWidth, (float)Rectangle4.ActualHeight);
            
            // Parent visuals to XAML rectangles
            ElementCompositionPreview.SetElementChildVisual(Rectangle1, _vis1);
            ElementCompositionPreview.SetElementChildVisual(Rectangle2, _vis2);
            ElementCompositionPreview.SetElementChildVisual(Rectangle3, _vis3);
            ElementCompositionPreview.SetElementChildVisual(Rectangle4, _vis4);
            
            //create the scale & offset animation

            Vector3KeyFrameAnimation offsetAnim_r1 = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnim_r1.InsertKeyFrame(1, new Vector3((float)Rectangle1.ActualWidth, (float)Rectangle1.ActualHeight, 0));
            offsetAnim_r1.Duration = TimeSpan.FromSeconds(6);
            
            // Instantiate animation
            //TODO update to animate gradient stops instaed of scale
            _scaleAnim = _compositor.CreateVector3KeyFrameAnimation();
            _scaleAnim.InsertKeyFrame(0, new Vector3(0, 1, 0));
            _scaleAnim.InsertKeyFrame(.5f, new Vector3(1, 1, 0));
            _scaleAnim.InsertKeyFrame(1, new Vector3(0, 1, 0));
            _scaleAnim.Duration = TimeSpan.FromSeconds(2);
            
            // animation of color stops
            _colorAnimGradientStop1 = _compositor.CreateColorKeyFrameAnimation();
            _colorAnimGradientStop1.InsertKeyFrame(.5f, Colors.Honeydew);
            _colorAnimGradientStop1.Duration = TimeSpan.FromSeconds(4);

            _colorAnimGradientStop2 = _compositor.CreateColorKeyFrameAnimation();
            _colorAnimGradientStop2.InsertKeyFrame(.5f, Colors.DeepPink);
            _colorAnimGradientStop2.Duration = TimeSpan.FromSeconds(4);

            _offsetAnim = _compositor.CreateScalarKeyFrameAnimation();
            _offsetAnim.Duration = TimeSpan.FromSeconds(1);

            // when the buttons of text are pressed, the brush will change colors. Below is the set up for animation
            _changeb1stop1 = _compositor.CreateColorKeyFrameAnimation();
            _changeb1stop1.InsertKeyFrame(.5f, Colors.LightSkyBlue);
            _changeb1stop1.Duration = TimeSpan.FromSeconds(2);

            _changeb1stop2 = _compositor.CreateColorKeyFrameAnimation();
            _changeb1stop2.InsertKeyFrame(.5f, Colors.Teal);
            _changeb1stop2.Duration = TimeSpan.FromSeconds(2);
        }

        /*
         * Run animation on target visual brush
         */

        private void Pointer_Pressed(object sender, RoutedEventArgs e)
        {
            pointerPressedChangeColors(_gradientStop1, _gradientStop2);
        }

        private void pointerPressedChangeColors(CompositionColorGradientStop target, CompositionColorGradientStop target1)
        {

            if (target.Color == Colors.DeepPink || target.Color == Colors.Honeydew)
            {
                target.StartAnimation(nameof(target.Color), _changeb1stop1);
                target1.StartAnimation(nameof(target1.Color), _changeb1stop2);
            }
            else if (target.Color == Colors.LightSkyBlue || target.Color == Colors.Teal)
            {
                target1.StartAnimation(nameof(target1.Color), _colorAnimGradientStop1);
                target.StartAnimation(nameof(target.Color), _colorAnimGradientStop2);
            }
        }
        private void AnimateGradient(SpriteVisual target)
        {
            target.StartAnimation("Scale", _scaleAnim);
            //target.StartAnimation("Offset", offsetAnim_r1);
        }

        private void AnimateBrushStop1(CompositionColorGradientStop target, CompositionColorGradientStop target1)
        {
            if (target.Color == Colors.DeepPink)
            {
                target.StartAnimation(nameof(target.Color), _colorAnimGradientStop1);
                target1.StartAnimation(nameof(target1.Color), _colorAnimGradientStop2);
            }
            else if (target.Color == Colors.Teal)
            {
                target.StartAnimation(nameof(target.Color), _changeb1stop1);
                target1.StartAnimation(nameof(target1.Color), _changeb1stop2);
            }

        }

        private void ChangeBrushBack(CompositionColorGradientStop target, CompositionColorGradientStop target1)
        {
            if (target.Color == Colors.Honeydew)
            {
                target1.StartAnimation(nameof(target1.Color), _colorAnimGradientStop1);
                target.StartAnimation(nameof(target.Color), _colorAnimGradientStop2);
            }
            else if (target.Color == Colors.LightSkyBlue)
            {
                target1.StartAnimation(nameof(target1.Color), _changeb1stop1);
                target.StartAnimation(nameof(target.Color), _changeb1stop2);
            }
        }

        private void AnimateOffset(SpriteVisual target)
        {
            float endPoint = target.Size.X;
            float startPoint = target.Size.X - target.Size.X;

            if (target.Offset.X == startPoint)
            {
                target.AnchorPoint = new Vector2(1f, 0f);
                _offsetAnim.InsertKeyFrame(1, endPoint);

            }
            else if (target.Offset.X == endPoint)
            {
                target.AnchorPoint = new Vector2(0f, 0f);
                _offsetAnim.InsertKeyFrame(1, startPoint);
            }
            target.StartAnimation("Offset.X", _offsetAnim);

        }

        // Animate the Visual backing the TextBlock when the Canvas containing the TextBlock is entered. 
        // When the first and last TextBlock is entered via pointer the current LinearGradient brush applied will switch the colors of its stops
        private void c1_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            AnimateBrushStop1(_gradientStop1, _gradientStop2);
            AnimateGradient(_vis1);
            AnimateOffset(_vis1);
        }

        private void c2_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AnimateGradient(_vis2);
            AnimateOffset(_vis2);
        }

        private void c3_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AnimateGradient(_vis3);
            AnimateOffset(_vis3);
        }

        private void c4_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeBrushBack(_gradientStop1, _gradientStop2);
            AnimateGradient(_vis4);
            AnimateOffset(_vis4);
        }
    }
}
