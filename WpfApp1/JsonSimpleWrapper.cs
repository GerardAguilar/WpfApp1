using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchAuto
{
    public class JsonSimpleWrapper
    {
        private JArray events;

        public JsonSimpleWrapper()
        {
            events = new JArray();
        }

        public void WriteEvent(Coordinate coordinate)
        {
            JObject newEvent = new JObject(
                new JProperty("x", coordinate.x),
                new JProperty("y", coordinate.y),
                new JProperty("timestamp", coordinate.timestamp),
                new JProperty("timeDiff", 0)
                );

            //{
            //  "x": "100",
            //  "y": "100",
            //  "timestamp": "123456789",
            //  "timeDiff": "70"
            //}

            events.Add(newEvent);
        }

        public void WriteEvent(Coordinate coordinate, Coordinate beforeCoordinate) {
            JObject newEvent = new JObject(
            new JProperty("x", coordinate.x),
            new JProperty("y", coordinate.y),
            new JProperty("timestamp", coordinate.timestamp),
            new JProperty("timeDiff", coordinate.timestamp - beforeCoordinate.timestamp)
            );

            //{
            //  "x": "100",
            //  "y": "100",
            //  "timestamp": "123456789",
            //  "timeDiff": "2000"
            //}

            events.Add(newEvent);
        }

        public void WriteEvents(List<Coordinate> coordinates)
        {
            if (coordinates.Count > 0) {
                WriteEvent(coordinates[0]);//sets the first coordinate timeDiff to zero as well
                for (int i = 1; i < coordinates.Count; i++)
                {
                    WriteEvent(coordinates[i], coordinates[i - 1]);
                }
            }
        }

        public void SaveEvents(String installDirectory)
        {
            Console.WriteLine("Saving Events to: " + installDirectory);
            //File.WriteAllText(installDirectory + "events.json", events.ToString());
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json| All files(*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, events.ToString());
        }

        public void SaveEvents(String installDirectory, String eventFolder, bool noSaveDialog) {
            String installFolder = installDirectory + "\\" + eventFolder;
            Directory.CreateDirectory(installFolder);
            File.WriteAllText(installFolder + "\\" + eventFolder + ".json", events.ToString());
        }

        //loads a single event
        public List<Coordinate> LoadEvent(String targetFile)
        {
            List<Coordinate> result = JsonConvert.DeserializeObject<List<Coordinate>>(File.ReadAllText(targetFile));
            //for (int i = 0; i < result.Count; i++) {
            //    Console.WriteLine("x:" + result[i].x); 
            //}
            return result;
        }

        public String GetEventsAsString() {
            return events.ToString();
        }

        public void ClearEvents()
        {
            events = new JArray();
        }
    }
}
