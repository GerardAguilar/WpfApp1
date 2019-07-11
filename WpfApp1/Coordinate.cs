using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchAuto
{
    public class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        public int timestamp { get; set; }
        public long timeDiff { get; set; }

        public Coordinate()
        {
            x = 0;
            y = 0;
            timestamp = 0;
            timeDiff = 0;
        }

        public Coordinate(int newX, int newY)
        {
            setX(newX);
            setY(newY);
            timestamp = 0;
            timeDiff = 0;
        }

        public Coordinate(int newX, int newY, int time)
        {
            setX(newX);
            setY(newY);
            recordTimestamp(time);
            setTimeDiff(0);
        }

        public Coordinate(int newX, int newY, int time, long diff)
        {
            setX(newX);
            setY(newY);
            recordTimestamp(time);
            setTimeDiff(diff);
        }

        public int getX()
        {
            return x;
        }
        public void setX(int newX)
        {
            x = newX;
        }
        public int getY()
        {
            return y;
        }
        public void setY(int newY)
        {
            y = newY;
        }
        public int getTimestamp()
        {
            return timestamp;
        }
        public void recordTimestamp(int time)
        {
            timestamp = time;
        }

        public void printCoordinate()
        {
            //System.out.println("Coordinate: " + getX() + ", " + getY() + " @" + timestamp.getTime());
            Console.WriteLine("Coordinate: " + getX() + ", " + getY() + " @" + timestamp);
        }

        /***
         * @param other
         * @return milliseconds
         */
        public long getTimeDifference(Coordinate other)
        {
            long diff;
            if (other == null)
            {
                diff = 0;
            }
            else
            {
                diff = timestamp - other.getTimestamp();
            }
            return diff;
        }

        public void setTimeDiff(long diff)
        {
            timeDiff = diff;
        }

        public long getTimeDiff()
        {
            return timeDiff;
        }
    }
}
