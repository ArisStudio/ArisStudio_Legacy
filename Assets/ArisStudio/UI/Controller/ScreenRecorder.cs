// using UnityEngine;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Threading;
// using UnityEditor;
// using UnityEditor.Media;
//
// class BitmapEncoder
// {
//     public static void WriteBitmap(Stream stream, int width, int height, byte[] imageData)
//     {
//         using (BinaryWriter bw = new BinaryWriter(stream))
//         {
//             // define the bitmap file header
//             bw.Write((UInt16)0x4D42); // bfType;
//             bw.Write((UInt32)(14 + 40 + (width * height * 4))); // bfSize;
//             bw.Write((UInt16)0); // bfReserved1;
//             bw.Write((UInt16)0); // bfReserved2;
//             bw.Write((UInt32)14 + 40); // bfOffBits;
//
//             // define the bitmap information header
//             bw.Write((UInt32)40); // biSize;
//             bw.Write((Int32)width); // biWidth;
//             bw.Write((Int32)height); // biHeight;
//             bw.Write((UInt16)1); // biPlanes;
//             bw.Write((UInt16)32); // biBitCount;
//             bw.Write((UInt32)0); // biCompression;
//             bw.Write((UInt32)(width * height * 4)); // biSizeImage;
//             bw.Write((Int32)0); // biXPelsPerMeter;
//             bw.Write((Int32)0); // biYPelsPerMeter;
//             bw.Write((UInt32)0); // biClrUsed;
//             bw.Write((UInt32)0); // biClrImportant;
//
//             // switch the image data from RGB to BGR
//             for (int imageIdx = 0; imageIdx < imageData.Length; imageIdx += 3)
//             {
//                 bw.Write(imageData[imageIdx + 2]);
//                 bw.Write(imageData[imageIdx + 1]);
//                 bw.Write(imageData[imageIdx + 0]);
//                 bw.Write((byte)255);
//             }
//         }
//     }
// }
//
// [RequireComponent(typeof(Camera))]
// public class ScreenRecorder : MonoBehaviour
// {
//     // Public Properties
//     public int maxFrames;
//     public int frameRate = 30;
//
//     // The Encoder Thread
//     private Thread encoderThread;
//
//     // Texture Readback Objects
//     private RenderTexture tempRenderTexture;
//     private Texture2D tempTexture2D;
//
//     // Timing Data
//     private float captureFrameTime;
//     private float lastFrameTime;
//     private int frameNumber;
//     private int savingFrameNumber;
//
//     // Encoder Thread Shared Resources
//     private Queue<byte[]> frameQueue;
//     private string persistentDataPath;
//     private int screenWidth;
//     private int screenHeight;
//     private bool threadIsProcessing;
//     private bool terminateThreadWhenDone;
//
//     private VideoTrackAttributes vidAttr;
//     private AudioTrackAttributes audAttr;
//     private MediaEncoder videoEncoder;
//
//     void Start()
//     {
//         // Set target frame rate (optional)
//         Application.targetFrameRate = frameRate;
//
//         var rootPath = Directory.GetParent(Application.dataPath).ToString();
//         persistentDataPath = Path.Combine(rootPath, "ScreenRecorder");
//
//         if (!System.IO.Directory.Exists(persistentDataPath))
//         {
//             System.IO.Directory.CreateDirectory(persistentDataPath);
//         }
//
//         // Prepare textures and initial values
//         screenWidth = GetComponent<Camera>().pixelWidth;
//         screenHeight = GetComponent<Camera>().pixelHeight;
//
//         vidAttr = new VideoTrackAttributes
//         {
//             bitRateMode = VideoBitrateMode.Medium,
//             frameRate = new MediaRational(Application.targetFrameRate),
//             width = (uint)screenWidth,
//             height = (uint)screenHeight,
//             includeAlpha = false
//         };
//
//         audAttr = new AudioTrackAttributes
//         {
//             sampleRate = new MediaRational(48000),
//             channelCount = 1
//         };
//
//         videoEncoder = new MediaEncoder(persistentDataPath + "/video" + DateTime.Now.Minute + ".mp4", vidAttr, audAttr);
//
//         tempRenderTexture = new RenderTexture(screenWidth, screenHeight, 0);
//         tempTexture2D = new Texture2D(screenWidth, screenHeight, TextureFormat.RGBA32, false);
//         frameQueue = new Queue<byte[]>();
//
//         frameNumber = 0;
//         savingFrameNumber = 0;
//
//         captureFrameTime = 1.0f / (float)frameRate;
//         lastFrameTime = Time.time;
//
//         // Kill the encoder thread if running from a previous execution
//         if (encoderThread != null && (threadIsProcessing || encoderThread.IsAlive))
//         {
//             threadIsProcessing = false;
//             encoderThread.Join();
//         }
//
//         // Start a new encoder thread
//         threadIsProcessing = true;
//         // encoderThread = new Thread(EncodeAndSave);
//         // encoderThread.Start();
//     }
//
//     void OnDisable()
//     {
//         videoEncoder.Dispose();
//         // Reset target frame rate
//         Application.targetFrameRate = -1;
//
//         // Inform thread to terminate when finished processing frames
//         terminateThreadWhenDone = true;
//     }
//
//     void OnRenderImage(RenderTexture source, RenderTexture destination)
//     {
//         if (frameNumber <= maxFrames)
//         {
//             // Check if render target size has changed, if so, terminate
//             if (source.width != screenWidth || source.height != screenHeight)
//             {
//                 threadIsProcessing = false;
//                 this.enabled = false;
//                 throw new UnityException("ScreenRecorder render target size has changed!");
//             }
//
//             // Calculate number of video frames to produce from this game frame
//             // Generate 'padding' frames if desired framerate is higher than actual framerate
//             float thisFrameTime = Time.time;
//             int framesToCapture = ((int)(thisFrameTime / captureFrameTime)) - ((int)(lastFrameTime / captureFrameTime));
//
//             // Capture the frame
//             if (framesToCapture > 0)
//             {
//                 Graphics.Blit(source, tempRenderTexture);
//
//                 RenderTexture.active = tempRenderTexture;
//                 tempTexture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
//                 tempTexture2D.Apply();
//                 videoEncoder.AddFrame(tempTexture2D);
//                 RenderTexture.active = null;
//             }
//
//             // Add the required number of copies to the queue
//             for (int i = 0; i < framesToCapture && frameNumber <= maxFrames; ++i)
//             {
//                 frameNumber++;
//
//                 if (frameNumber % frameRate == 0)
//                 {
//                     print("Frame " + frameNumber);
//                 }
//             }
//
//             lastFrameTime = thisFrameTime;
//         }
//         else //keep making screenshots until it reaches the max frame amount
//         {
//             // Inform thread to terminate when finished processing frames
//             terminateThreadWhenDone = true;
//
//             // Disable script
//             this.enabled = false;
//         }
//
//         // Passthrough
//         Graphics.Blit(source, destination);
//     }
// }

