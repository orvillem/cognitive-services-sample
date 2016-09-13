// Sample code added to show description, crop and emotions in one place


using System;
using System.IO;
using System.Threading.Tasks;

// -----------------------------------------------------------------------
// KEY SAMPLE CODE STARTS HERE
// Use the following namesapce for VisionServiceClient
// -----------------------------------------------------------------------
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Windows.Media.Imaging;
// -----------------------------------------------------------------------
// KEY SAMPLE CODE ENDS HERE
// -----------------------------------------------------------------------

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Interaction logic for DescribeCropFeelPage.xaml
    /// </summary>
    public partial class DescribeCropFeelPage : ImageScenarioPage
    {
        public DescribeCropFeelPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
        }

        /// <summary>
        /// Uploads the image to Project Oxford and performs analysis
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns></returns>
        private async Task<AnalysisResult> UploadAndAnalyzeImage(string imageFilePath)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            Log("VisionServiceClient is created");

            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                //
                // Analyze the image for all visual features
                //
                Log("Calling VisionServiceClient.AnalyzeImageAsync()...");
                VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
                AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageFileStream, visualFeatures);
                return analysisResult;
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Uploads the image to Project Oxford and generates a thumbnail
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <param name="width">Width of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="height">Height of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="smartCropping">Boolean flag for enabling smart cropping.</param>
        /// <returns></returns>
        private async Task<byte[]> UploadAndThumbnailImage(string imageFilePath, int width, int height, bool smartCropping)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            //Log("VisionServiceClient is created");

            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                //
                // Upload an image and generate a thumbnail
                //
                Log("Calling VisionServiceClient.GetThumbnailAsync()...");
                return await VisionServiceClient.GetThumbnailAsync(imageFileStream, width, height, smartCropping);
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a url to Project Oxford and performs analysis
        /// </summary>
        /// <param name="imageUrl">The url of the image to analyze</param>
        /// <returns></returns>
        private async Task<AnalysisResult> AnalyzeUrl(string imageUrl)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            Log("VisionServiceClient is created");

            //
            // Analyze the url for all visual features
            //
            Log("Calling VisionServiceClient.AnalyzeImageAsync()...");
            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
            AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
            return analysisResult;

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a url to Project Oxford and generates a thumbnail
        /// </summary>
        /// <param name="imageUrl">The url of the image to generate a thumbnail for</param>
        /// <param name="width">Width of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="height">Height of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="smartCropping">Boolean flag for enabling smart cropping.</param>
        /// <returns></returns>
        private async Task<byte[]> ThumbnailUrl(string imageUrl, int width, int height, bool smartCropping)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            //Log("VisionServiceClient is created");

            //
            // Generate a thumbnail for the given url
            //
            Log("Calling VisionServiceClient.GetThumbnailAsync()...");
            byte[] thumbnail = await VisionServiceClient.GetThumbnailAsync(imageUrl, width, height, smartCropping);
            return thumbnail;

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Perform the work for this scenario
        /// </summary>
        /// <param name="imageUri">The URI of the image to run against the scenario</param>
        /// <param name="upload">Upload the image to Project Oxford if [true]; submit the Uri as a remote url if [false];</param>
        /// <returns></returns>
        protected override async Task DoWork(Uri imageUri, bool upload)
        {
            _status.Text = "Analyzing the image...";

            //
            // Either upload an image, or supply a url
            //
            AnalysisResult analysisResult;
            bool useSmartCropping = true;
            byte[] thumbnailWide;
            byte[] thumbnailTall;
            if (upload)
            {
                analysisResult = await UploadAndAnalyzeImage(imageUri.LocalPath);
                // image path, width, height, use smart cropping?
                thumbnailWide = await UploadAndThumbnailImage(imageUri.LocalPath, 300, 150, useSmartCropping);
                thumbnailTall = await UploadAndThumbnailImage(imageUri.LocalPath, 150, 300, useSmartCropping);
            }
            else
            {
                analysisResult = await AnalyzeUrl(imageUri.AbsoluteUri);
                // image path, width, height, use smart cropping?
                thumbnailWide = await ThumbnailUrl(imageUri.AbsoluteUri, 300, 150, useSmartCropping);
                thumbnailTall = await ThumbnailUrl(imageUri.AbsoluteUri, 150, 300, useSmartCropping);
            }
            _status.Text = "Analysis and cropping is complete";

            //
            // Log analysis result in the log window
            //
            Log("");
            Log("Analysis Result:");
            LogAnalysisResult(analysisResult);

            //
            // Display the tall and wide thumbnails in the GUI
            //
            //
            // Show the generated thumbnail in the GUI
            //
            MemoryStream msWide = new MemoryStream(thumbnailWide);
            msWide.Seek(0, SeekOrigin.Begin);

            BitmapImage thumbSourceWide = new BitmapImage();
            thumbSourceWide.BeginInit();
            thumbSourceWide.CacheOption = BitmapCacheOption.None;
            thumbSourceWide.StreamSource = msWide;
            thumbSourceWide.EndInit();
            _thumbPreviewWide.Source = thumbSourceWide;

            //
            // Show the generated thumbnail in the GUI
            //
            MemoryStream msTall = new MemoryStream(thumbnailTall);
            msTall.Seek(0, SeekOrigin.Begin);

            BitmapImage thumbSourceTall = new BitmapImage();
            thumbSourceTall.BeginInit();
            thumbSourceTall.CacheOption = BitmapCacheOption.None;
            thumbSourceTall.StreamSource = msTall;
            thumbSourceTall.EndInit();
            _thumbPreviewTall.Source = thumbSourceTall;
        }
    }
}
