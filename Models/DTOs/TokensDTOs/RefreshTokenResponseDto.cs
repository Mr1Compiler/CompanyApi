﻿namespace CompanyApi.Models.DTOs.Tokens
{
	public class RefreshTokenResponseDto
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime AccessTokenExpiresAt { get; set; }
		public DateTime RefreshTokenExpiresAt { get; set; }
	}
}
