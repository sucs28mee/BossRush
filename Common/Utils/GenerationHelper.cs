﻿using BossRush.Common.WorldGenOverhaul.XinimVer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.Utils;

internal partial class GenerationHelper {
	public static bool CoordinatesOutOfBounds(int i, int j) => i >= Main.maxTilesX || j >= Main.maxTilesY || i < 0 || j < 0;

	public static void FastPlaceTile(int i, int j, ushort tileType) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Tile tile = Main.tile[i, j];
		tile.TileType = tileType;
		tile.Get<TileWallWireStateData>().HasTile = true;
	}

	public static void FastPlaceTile(int i, int j, int tileType) {
		FastPlaceTile(i, j, (ushort)tileType);
	}

	public static void FastRemoveTile(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].Get<TileWallWireStateData>().HasTile = false;
	}

	public static void FastPlaceWall(int i, int j, int wallType) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].WallType = (ushort)wallType;
	}

	public static void FastRemoveWall(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].WallType = WallID.None;
	}

	public static void ForEachInRectangle(Rectangle rectangle, Action<int, int> action) {
		for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++) {
			for (int j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++) {
				action(i, j);
			}
		}
	}

	public static void ForEachInRectangle(int i, int j, int width, int height, Action<int, int> action) {
		ForEachInRectangle(new Rectangle(i, j, width, height), action);
	}

	public static void ForEachInCircle(int i, int j, int width, int height, Action<int, int> action) {
		ForEachInRectangle(
			i - width / 2,
			j - height / 2,
			width,
			height,
			(iLocal, jLocal) => {
				if (MathF.Pow((iLocal - i) / (width * 0.5f), 2) + MathF.Pow((jLocal - j) / (height * 0.5f), 2) - 1 >= 0) {
					return;
				}

				action(iLocal, jLocal);
			}
		);
	}
	/// <summary>
	/// Use this for easy place tile in the world in 24x24 grid like
	/// </summary>
	/// <param name="x">The starting X position</param>
	/// <param name="y">The Starting Y position</param>
	/// <param name="dragX">Use this if you want to select multiple grid on X axis</param>
	/// <param name="dragY">Use this if you want to select multiple grid on Y axis</param>
	/// <returns></returns>
	public static Rectangle GridPositionInTheWorld24x24(int x, int y, float dragX = 1, float dragY = 1)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X * x, RogueLikeWorldGen.GridPart_Y * y, (int)(RogueLikeWorldGen.GridPart_X * dragX), (int)(RogueLikeWorldGen.GridPart_Y * dragY));

	public static Rectangle GridPositionInTheWorld48x48(int x, int y, float dragX = 1, float dragY = 1)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X * x, RogueLikeWorldGen.GridPart_Y * y, (int)(RogueLikeWorldGen.GridPart_X / 2 * dragX), (int)(RogueLikeWorldGen.GridPart_Y / 2 * dragY));
	public static void ForEachInCircle(int i, int j, int radius, Action<int, int> action) {
		ForEachInCircle(i, j, radius * 2, radius * 2, action);
	}
}
