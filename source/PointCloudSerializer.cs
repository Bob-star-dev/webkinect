using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;

namespace KinectServer
{
    // Handles point cloud serialization for 3D scanning.
    public static class PointCloudSerializer
    {
        [DataContract]
        class PointCloudData
        {
            [DataMember(Name = "width")]
            public int Width { get; set; }

            [DataMember(Name = "height")]
            public int Height { get; set; }

            [DataMember(Name = "points")]
            public List<Point3D> Points { get; set; }
        }

        [DataContract]
        class Point3D
        {
            [DataMember(Name = "x")]
            public float X { get; set; }

            [DataMember(Name = "y")]
            public float Y { get; set; }

            [DataMember(Name = "z")]
            public float Z { get; set; }

            [DataMember(Name = "r")]
            public byte R { get; set; }

            [DataMember(Name = "g")]
            public byte G { get; set; }

            [DataMember(Name = "b")]
            public byte B { get; set; }
        }

        // Serializes depth frame to point cloud with color mapping.
        public static string Serialize(this DepthImageFrame depthFrame, ColorImageFrame colorFrame, CoordinateMapper mapper)
        {
            if (depthFrame == null)
                return "{\"width\":0,\"height\":0,\"points\":[]}";
            
            int width = depthFrame.Width;
            int height = depthFrame.Height;
            short[] depthData = new short[depthFrame.PixelDataLength];
            byte[] colorData = null;
            ColorImagePoint[] colorPoints = null;

            depthFrame.CopyPixelDataTo(depthData);

            // Get color data if available
            if (colorFrame != null)
            {
                colorData = new byte[colorFrame.PixelDataLength];
                colorFrame.CopyPixelDataTo(colorData);
                colorPoints = new ColorImagePoint[depthData.Length];
                mapper.MapDepthFrameToColorFrame(
                    DepthImageFormat.Resolution640x480Fps30,
                    depthData,
                    ColorImageFormat.RgbResolution640x480Fps30,
                    colorPoints);
            }

            PointCloudData pointCloud = new PointCloudData
            {
                Width = width,
                Height = height,
                Points = new List<Point3D>()
            };

            // Convert depth pixels to 3D points
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int depthIndex = x + (y * width);
                    int depth = depthData[depthIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                    // Filter out invalid depth values
                    if (depth >= Constants.MIN_DEPTH_DISTANCE && depth <= Constants.MAX_DEPTH_DISTANCE)
                    {
                        DepthImagePoint depthPoint = new DepthImagePoint
                        {
                            X = x,
                            Y = y,
                            Depth = depth
                        };

                        SkeletonPoint skeletonPoint = mapper.MapDepthPointToSkeletonPoint(
                            DepthImageFormat.Resolution640x480Fps30,
                            depthPoint);

                        Point3D point = new Point3D
                        {
                            X = skeletonPoint.X,
                            Y = skeletonPoint.Y,
                            Z = skeletonPoint.Z
                        };

                        // Add color if available
                        if (colorFrame != null && colorPoints != null)
                        {
                            ColorImagePoint colorPoint = colorPoints[depthIndex];
                            if (colorPoint.X >= 0 && colorPoint.X < colorFrame.Width &&
                                colorPoint.Y >= 0 && colorPoint.Y < colorFrame.Height)
                            {
                                int colorIndex = (colorPoint.X + (colorPoint.Y * colorFrame.Width)) * 4;
                                if (colorIndex + 2 < colorData.Length)
                                {
                                    // BGRA format
                                    point.B = colorData[colorIndex];
                                    point.G = colorData[colorIndex + 1];
                                    point.R = colorData[colorIndex + 2];
                                }
                            }
                        }
                        else
                        {
                            // Use depth as grayscale color
                            byte intensity = (byte)(255 - (255 * (depth - Constants.MIN_DEPTH_DISTANCE) / Constants.MAX_DEPTH_DISTANCE_OFFSET));
                            point.R = intensity;
                            point.G = intensity;
                            point.B = intensity;
                        }

                        pointCloud.Points.Add(point);
                    }
                }
            }

            return Serialize(pointCloud);
        }

        // Serializes an object to JSON.
        private static string Serialize(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
