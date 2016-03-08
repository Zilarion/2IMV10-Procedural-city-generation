﻿using UnityEngine;

namespace ProceduralCity
{
	public class Map
	{
		private static float latOrigin = -90, 
							 lonOrigin = -180;

		public static void setOrigin(float lat, float lon) {
			latOrigin = lat;
			lonOrigin = lon;
			Debug.Log ("Loaded new origin: (" + latOrigin + ", " + lonOrigin + ")");
		}

		public static void loadOrigin() {
			float minLat = 90;
			float minLon = 180;
			foreach (long key in Data.Instance.ways.Keys) {
				OsmWay ways = (OsmWay)Data.Instance.ways [key];
				for (int i = 0; i < ways.getNumberOfNodes (); i++) {
					OsmNodeReference node = ways.getNodeReference (i);
					float lat = node.getLattitude ();
					float lon = node.getLongitude ();
					if (lat < minLat) minLat = lat;
					if (lon < minLon) minLon = lon;
				}
			}
			setOrigin (minLat, minLon);
		}

		public static Vector3 getVectorFromOrigin(float lat, float lon, float height = 0f) {
			float latDiff = getLocationDistance (latOrigin, lonOrigin, lat, lonOrigin);
			float lonDiff = getLocationDistance (latOrigin, lonOrigin, latOrigin, lon);
			return new Vector3 (lonDiff, height, latDiff);
		}

		public static float getLocationDistance(float lat1, float lon1, float lat2, float lon2) {
			float  R = 6371000; // metres
			float φ1 = lat1 * Mathf.Deg2Rad;
			float φ2 = lat2 * Mathf.Deg2Rad;
			float Δφ = (lat2-lat1) * Mathf.Deg2Rad;
			float Δλ = (lon2-lon1) * Mathf.Deg2Rad;

			var a = Mathf.Sin(Δφ/2) * Mathf.Sin(Δφ/2) +
				Mathf.Cos(φ1) * Mathf.Cos(φ2) *
				Mathf.Sin(Δλ/2) * Mathf.Sin(Δλ/2);
			var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1-a));

			return R * c;
		}
	}
}

