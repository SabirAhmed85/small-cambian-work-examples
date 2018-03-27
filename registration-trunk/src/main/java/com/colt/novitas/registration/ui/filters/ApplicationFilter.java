package com.colt.novitas.registration.ui.filters;

import java.io.IOException;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.filter.OncePerRequestFilter;

public class ApplicationFilter extends OncePerRequestFilter {

	@Override
	protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain)
			throws ServletException, IOException {
		response.addHeader("Pragma", "no-cache");
		response.addHeader("Cache-Control", "no-cache, no-store");
		response.addHeader("Expires", "0");
		response.addHeader("X-Content-Type-Options", "nosniff");
		response.addHeader("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");
		response.addHeader("Server", "novitas");
		response.addHeader("X-Frame-Options", "DENY");
		response.addHeader("X-XSS-Protection", "1; mode=block");
		filterChain.doFilter(request, response);
	}

}