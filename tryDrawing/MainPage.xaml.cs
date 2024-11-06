using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;



//dotnet build "Cherkes_Ivan_Drawing_App.sln" -t:Run -f net8.0-ios -p:_DeviceName=:v2:udid=BD2DB845-975A-4F46-8DB1-55537A113A4C -- for iphone
//dotnet build "Cherkes_Ivan_Drawing_App.sln" -t:Run -f net8.0-ios

//dotnet build "Cherkes_Ivan_Drawing_App.sln" -t:Run -f net8.0-maccatalyst
//dotnet build -t:Run -f net8.0-maccatalyst 


namespace tryDrawing
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<IDrawingLine> Lines { get; set; }

        public MainPage()
        {
            InitializeComponent();
            //adding event handler for window change
            this.SizeChanged += OnWindowChange;
            Title = "";
            Lines = new ObservableCollection<IDrawingLine>();
            //binding lines collection to draw view
            DrawView.Lines = Lines;

            DrawView.DrawingLineCompleted += DrawView_DrawingLineCompleted;
        }

        void DrawView_DrawingLineCompleted(System.Object sender, CommunityToolkit.Maui.Core.DrawingLineCompletedEventArgs e)
        {
            Lines.Add(e.LastDrawingLine);
            
        }



        void Clear_Btn_Clicked(System.Object sender, System.EventArgs e)
        {
            DrawView.Clear();
            DrawView.BackgroundColor = Colors.White;
        }

        void Change_Color_Btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Random random = new Random();
            int red = random.Next(0, 256);
            int green = random.Next(0, 256);
            int blue = random.Next(0, 256);

            Color color = new Color(red, green , blue );

            // debugging
            //Console.WriteLine($"New Color: {color}");
            //ColorText.Text = color + "";
            DrawView.LineColor = color;
            
        }

        void Change_Canvas_Color_Btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Random random = new Random();
            int red = random.Next(0, 256);
            int green = random.Next(0, 256);
            int blue = random.Next(0, 256);

            Color color = new Color(red, green, blue);
            DrawView.BackgroundColor = color;
        }

        private void OnWindowChange(object sender, EventArgs e)
        {
            double width = this.Width - 20;
            double height = this.Height - this.MainLayout.Height + DrawView.Height + 100;

            DrawView.HeightRequest = height;
            DrawView.WidthRequest = width;   
        }

        //one method for all three sliders
        void ColorSlider_ValueChanged(System.Object sender, Microsoft.Maui.Controls.ValueChangedEventArgs e)
        {
         
            //retrieving values from sliders to use later for line color
            int red = (int)RedSlider.Value;
            int blue = (int)BlueSlider.Value;
            int green = (int)GreenSlider.Value;

            //updating labels based on values
            RedLabel.Text = "Red: " + red;
            GreenLabel.Text = "Green: " + green;
            BlueLabel.Text = "Blue: " + blue;

            //creating new color based on sliders' values and changing brush color
            Color color = new Color(red, green, blue);
            DrawView.LineColor = color;
            
        }

        void BrushSizeSlider_ValueChanged(System.Object sender, Microsoft.Maui.Controls.ValueChangedEventArgs e)
        {
            //retrieving value from slider
            int size = (int)BrushSizeSlider.Value;
            //updating label
            BrushSize.Text = "Brush size: " + size;
            //updating brush size
            DrawView.LineWidth = size;
        }

       


        //for some reason you have to press two times to undo action
        void Undo_Btn_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Lines.Count > 0)
            {
                Lines.RemoveAt(Lines.Count - 1);
            }
            
        }

        

        async void Save_Btn_Clicked(System.Object sender, System.EventArgs e)
        {

            try
            {
                // creating stream to save image
                using var stream = await DrawView.GetImageStream(1024, 1024);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                // saving works different for each system you might need a permission to access gallery (ios, macos)
#if IOS || MACCATALYST
        var image = new UIKit.UIImage(Foundation.NSData.FromArray(memoryStream.ToArray()));
        image.SaveToPhotosAlbum((img, error) => 
        {
            if (error != null)
            {
                // Handle any errors here
                Console.WriteLine($"Error saving image: {error.LocalizedDescription}");
            }
            else
            {
                Console.WriteLine("Image saved successfully!");
            }
        });

        //saving crashes app
#elif ANDROID
                var context = Platform.CurrentActivity;
                Android.Content.ContentResolver resolver = context.ContentResolver;
                Android.Content.ContentValues contentValues = new Android.Content.ContentValues();
                contentValues.Put(Android.Provider.MediaStore.IMediaColumns.DisplayName, "test.png");
                contentValues.Put(Android.Provider.MediaStore.IMediaColumns.MimeType, "image/png");
                contentValues.Put(Android.Provider.MediaStore.IMediaColumns.RelativePath, "DCIM/test");

                
                Android.Net.Uri imageUri = resolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);

                
                using (var os = resolver.OpenOutputStream(imageUri))
                {
                    
                    if (os == null)
                    {
                        
                        return;
                    }

                    
                    var bitmap = Android.Graphics.BitmapFactory.DecodeStream(new MemoryStream(memoryStream.ToArray()));
                    if (bitmap != null)
                    {
                        bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, os);
                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                }
#endif
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error saving drawing");
            }

            await DisplayAlert("Saved succesfully", "You have saved your drawing", "Ok");
        }

    }
}
