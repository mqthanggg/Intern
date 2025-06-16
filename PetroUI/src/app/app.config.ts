import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors, withXsrfConfiguration } from '@angular/common/http';
import { authInterceptor } from './http-intercepter.service';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes, withComponentInputBinding()),provideHttpClient(
    withXsrfConfiguration({
      cookieName: 'XSRF-COOKIE',
      headerName: 'XSRF-HEADER'
    }),
    withInterceptors([
      authInterceptor
    ])
  )]
};
