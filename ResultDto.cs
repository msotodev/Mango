﻿using System;

namespace CommonLibrary.Dtos
{
	public class ResultDto
	{
		public bool IsSuccess { get; set; }

		public string Message { get; set; } = string.Empty;
	}
}