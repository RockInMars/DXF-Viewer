﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DXF.Entities
{
    abstract class EntityComposite
    {
        public abstract EntityComposite draw(Canvas canvas);
    }
}
