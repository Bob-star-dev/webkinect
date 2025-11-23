using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectServer
{
    // Handles raw depth data serialization for 3D scanning.
    public static class RawDepthSerializer
    {
        // Serializes raw depth frame data as JSON.
        public static string SerializeRawDepth(this DepthImageFrame frame)
        {
            short[] depthData = new short[frame.PixelDataLength];
            frame.CopyPixelDataTo(depthData);

            // Convert to array of depth values (in millimeters)
            int[] depthValues = new int[depthData.Length];
            for (int i = 0; i < depthData.Length; i++)
            {
                // Extract depth value (remove player index bits)
                depthValues[i] = depthData[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;
            }

            // Build JSON manually (to avoid external dependencies)
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"type\":\"RawDepth\",");
            json.Append("\"width\":" + frame.Width + ",");
            json.Append("\"height\":" + frame.Height + ",");
            json.Append("\"minDepth\":" + Constants.MIN_DEPTH_DISTANCE + ",");
            json.Append("\"maxDepth\":" + Constants.MAX_DEPTH_DISTANCE + ",");
            json.Append("\"depth\":[");
            
            for (int i = 0; i < depthValues.Length; i++)
            {
                if (i > 0) json.Append(",");
                json.Append(depthValues[i]);
            }
            
            json.Append("]}");
            
            return json.ToString();
        }
    }
}

