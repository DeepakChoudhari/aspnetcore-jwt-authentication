
(function (global) {
  System.config({
    paths: {
      // Paths serve as alias
      'npm:': 'node_modules/'
    },
    // Map tells the System loader where to look for things
    map: {
      // Our app is within the app folder
      'app': 'app',
         
      // Angular bundles
      '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
      '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
      '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
      '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
      '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
      '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
      '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
      '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',
      'reflect-metadata': 'npm:reflect-metadata/Reflect.js',
      'zone.js': 'npm:zone.js/dist/zone.js',
      'es6-shim': 'npm:es6-shim/es6-shim.js',

      // Other libraries
      'rxjs': 'npm:rxjs'
    },
    // Packages tells the System loader how to load when no filename and/or no extension
    packages: {
      app: {
        main: 'main.js',
        defaultExtension: 'js'
      },
      rxjs: {
        defaultExtension: 'js'
      }
    }
  });
})(this);