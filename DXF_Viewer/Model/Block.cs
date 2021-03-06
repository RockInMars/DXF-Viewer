﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DXF.Extensions;
using System.Windows.Controls;
using System.Windows.Shapes;
using DXF.Viewer.Entities;

namespace DXF.Viewer.Model
{
    class Block 
    {
        public string handle = "";
        public string name = "";
        public string layer = "0";
        public Point anchor = new Point();
        public List<Entity> children = new List<Entity>();
        Canvas canvas;

        public Block(Canvas canvas)
        {
            this.canvas = canvas;
        }
        
        public Block draw(Insert insertion)
        {
            foreach(Entity child in children)
            {
                canvas.Children.Add(child.draw(insertion));
            }
            return this;
        }

        public Block parse(List<string> section, Schematic parent, Viewer viewer)
        {
            //First get Block group codes
            int i = 0;
            while(true)
            {
                if (section[i].Equals("  0")) break;
                switch(section[i])
                {
                    case "  2":
                        name = section[++i];
                        break;
                    case "  5":
                        handle = section[++i];
                        break;
                    case "  8":
                        layer = section[++i];
                        break;
                    case " 10":
                        anchor.X = section[++i].ConvertToDoubleWithCulture();
                        break;
                    case " 20":
                        anchor.Y = section[++i].ConvertToDoubleWithCulture();
                        break;
                    default:
                        i++;
                        break;
                }
            }

            //Populate child entities
            while(i < section.Count - 1)
            {
                List<string> entity = new List<string>();
                string type = section[++i];
                while (true)
                {
                    entity.Add(section[i]);
                    if (section[i].Equals("  0")) break;
                    i++;
                }
                try
                {
                    children.Add(EntityFactory.makeEntity(type, entity, parent, viewer));
                }
                catch(Exception ex)
                {
                    //Console.WriteLine(ex.StackTrace);
                }
            }

            return this;
        }

    }
}
