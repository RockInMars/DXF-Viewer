﻿using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using DXF.Extensions;
using DXF.Viewer.Model;
using DXF.Viewer.Util;

namespace DXF.Viewer.Entities
{
    class Circle : Entity
    {
        Point center = new Point(0,0);
        double radius = 0;

        public Circle(Schematic drawing, Viewer topLevelViewer)
            : base(drawing, topLevelViewer)
        {
        }

        public Circle(Schematic drawing, Viewer topLevelViewer, Point center, double radius)
            : base(drawing, topLevelViewer)
        {
            this.center = center;
            this.radius = radius;
        }

        public Circle(PointEntity point)
            :base (point.parent, point.viewer)
        {
            center = point.location;
            radius = PointEntity.POINT_RADIUS;
            layerName = point.layerName;
        }

        public override Path draw()
        {
            //init wpf object stack
            Path path = new Path();
            EllipseGeometry geometry = new EllipseGeometry();
            GeometryGroup group = new GeometryGroup();

            //translate DXF coords to wpf
            geometry.Center = ViewerHelper.mapToWPF(this.center, this.parent);
            geometry.RadiusX = radius;
            geometry.RadiusY = radius;

            //set up brush
            path.Stroke = new SolidColorBrush(ViewerHelper.getColor(layer.lineColor));
            path.StrokeDashArray = ViewerHelper.setLineType(layer.lineType);

            //package wpf elements
            group.Children.Add(geometry);
            path.Data = group;

            //set up line thickness binding
            Binding bind = new Binding();
            bind.Source = viewer;
            bind.Path = new PropertyPath("LineThickness");
            path.SetBinding(Path.StrokeThicknessProperty, bind);

            return path;
        }

        public override Entity parse(List<string> section)
        {
            gatherCodes(section);
            getCommonCodes();

            center.X = getCode(" 10", center.X);
            center.Y = getCode(" 20", center.Y);
            radius = getCode(" 40", radius);
            
            return this;
        }

        public override Path draw(Insert insert)
        {
            center.X += insert.anchor.X;
            center.Y += insert.anchor.Y;

            Path path = draw();

            center.X -= insert.anchor.X;
            center.Y -= insert.anchor.Y;

            path.RenderTransform = insert.getTransforms(path.RenderTransform);
            return path;
        }
    }
}
