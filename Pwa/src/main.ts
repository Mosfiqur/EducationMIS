import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));

// platformBrowserDynamic().bootstrapModule(AppModule)
//   .then(() => {
//     console.log('envFile',environment.production);
//     if (environment.production && 'serviceWorker' in navigator) {
//       console.log('entered...');
//       navigator.serviceWorker.getRegistration()
//         .then(active => {
//           console.log('registered...', active);
//           !active && navigator.serviceWorker.register('/ngsw-worker.js')
//         })
//         .catch(console.error);

//     }
//     else {
//       console.log('not in navigator...')
//       navigator.serviceWorker.register('/ngsw-worker.js')
//     }
//   });
