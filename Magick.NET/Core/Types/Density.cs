﻿//=================================================================================================
// Copyright 2013-2016 Dirk Lemstra <https://magick.codeplex.com/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in 
// compliance with the License. You may obtain a copy of the License at
//
//   http://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//=================================================================================================

using System;
using System.Globalization;


namespace ImageMagick
{
  ///<summary>
  /// Represents the density of an image.
  ///</summary>
  public sealed class Density : IEquatable<Density>
  {
    private void Initialize(string value)
    {
      Throw.IfNullOrEmpty("value", value);

      string[] values = value.Split(' ');
      Throw.IfTrue("value", values.Length > 2, "Invalid density specified.");

      if (values.Length == 2)
      {
        if (values[1].Equals("cm", StringComparison.OrdinalIgnoreCase))
          Units = DensityUnit.PixelsPerCentimeter;
        else if (values[1].Equals("inch", StringComparison.OrdinalIgnoreCase))
          Units = DensityUnit.PixelsPerInch;
        else
          throw new ArgumentException("Invalid density specified.");
      }

      string[] xyValues = values[0].Split('x');
      Throw.IfTrue("value", xyValues.Length > 2, "Invalid density specified.");

      double x;
      Throw.IfFalse("value", double.TryParse(xyValues[0], NumberStyles.Number, CultureInfo.InvariantCulture, out x), "Invalid density specified.");

      double y;
      if (xyValues.Length == 1)
        y = x;
      else
        Throw.IfFalse("value", double.TryParse(xyValues[1], NumberStyles.Number, CultureInfo.InvariantCulture, out y), "Invalid density specified.");

      X = x;
      Y = y;
    }

    ///<summary>
    /// Initializes a new instance of the Density class using the specified x and y.
    ///</summary>
    ///<param name="xy">The x and y.</param>
    public Density(double xy)
      : this(xy, xy)
    {
    }

    ///<summary>
    /// Initializes a new instance of the Density class using the specified x and y and units.
    ///</summary>
    ///<param name="xy">The x and y.</param>
    ///<param name="units">The units.</param>
    public Density(double xy, DensityUnit units)
      : this(xy, xy, units)
    {
    }

    ///<summary>
    /// Initializes a new instance of the Density class using the specified x and y.
    ///</summary>
    ///<param name="x">The x.</param>
    ///<param name="y">The y.</param>
    public Density(double x, double y)
      : this(x, y, DensityUnit.PixelsPerInch)
    {
    }

    ///<summary>
    /// Initializes a new instance of the Density class using the specified x and y and units.
    ///</summary>
    ///<param name="x">The x.</param>
    ///<param name="y">The y.</param>
    ///<param name="units">The units.</param>
    public Density(double x, double y, DensityUnit units)
    {
      X = x;
      Y = y;
      Units = units;
    }

    ///<summary>
    /// Initializes a new instance of the Density class using the specified string.
    ///</summary>
    ///<param name="value">PointD specifications in the form: &lt;x&gt;x&lt;y&gt;[inch/cm] (where x, y are numbers)</param>
    public Density(string value)
    {
      Initialize(value);
    }

    ///<summary>
    /// The units.
    ///</summary>
    public DensityUnit Units
    {
      get;
      private set;
    }

    ///<summary>
    /// The x resolution.
    ///</summary>
    public double X
    {
      get;
      private set;
    }

    ///<summary>
    /// The y resolution.
    ///</summary>
    public double Y
    {
      get;
      private set;
    }

    /// <summary>
    /// Determines whether the specified v instances are considered equal.
    /// </summary>
    /// <param name="left">The first v to compare.</param>
    /// <param name="right"> The second Density to compare.</param>
    /// <returns></returns>
    public static bool operator ==(Density left, Density right)
    {
      return Equals(left, right);
    }

    /// <summary>
    /// Determines whether the specified Density instances are not considered equal.
    /// </summary>
    /// <param name="left">The first Density to compare.</param>
    /// <param name="right"> The second Density to compare.</param>
    /// <returns></returns>
    public static bool operator !=(Density left, Density right)
    {
      return !Equals(left, right);
    }

    ///<summary>
    /// Determines whether the specified object is equal to the current density.
    ///</summary>
    ///<param name="obj">The object to compare this density with.</param>
    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;

      if (obj.GetType() != typeof(Density))
        return false;

      return Equals((Density)obj);
    }

    ///<summary>
    /// Determines whether the specified density is equal to the current density.
    ///</summary>
    ///<param name="other">The density to compare this density with.</param>
    public bool Equals(Density other)
    {
      if (ReferenceEquals(other, null))
        return false;

      if (ReferenceEquals(this, other))
        return true;

      return
        X == other.X &&
        Y == other.Y &&
        Units == other.Units;
    }

    ///<summary>
    /// Serves as a hash of this type.
    ///</summary>
    public override int GetHashCode()
    {
      return
        X.GetHashCode() ^
        Y.GetHashCode() ^
        Units.GetHashCode();
    }

    ///<summary>
    /// Returns a string that represents the current Density.
    ///</summary>
    public override string ToString()
    {
      string result = string.Format(CultureInfo.InvariantCulture, "{0}x{1}", X, Y);

      switch (Units)
      {
        case DensityUnit.PixelsPerCentimeter:
          return result + " cm";
        case DensityUnit.PixelsPerInch:
          return result + " inch";
        default:
          return result;
      }
    }
  }
}