'use strict';

describe('Service: localeService', function () {
  beforeEach(module('app'));
  beforeEach(module('ngMock'));
    //Add templates module to avoid http requests for html files
    //This is generated by html2js preprocessor
  beforeEach(module('templates'));

  var LocaleSrv, scope, rootScope, translate, LOCALES, filter, tmhDynamicLocale, window;

  beforeEach(inject(function (_localeService_, _$translate_, _$filter_, _LOCALES_, _tmhDynamicLocale_, _$window_) {
    LocaleSrv = _localeService_;
    filter = _$filter_;
    LOCALES = _LOCALES_;
    tmhDynamicLocale = _tmhDynamicLocale_;
    window = _$window_;

    spyOn(console, 'error');
  }));

  it('should exist', function () {
    expect(!!LocaleSrv).toBe(true);
  });

  it('console error should be registered if invalid locale is selected', function () {
    LocaleSrv.setLocale('deert');
    expect(console.error).toHaveBeenCalled();
  });

  it('console error should not be called if valid locale is selected', function () {
    LocaleSrv.setLocale('fr');
    expect(console.error).not.toHaveBeenCalled();
  });

  it('getLocale method should return correct locale after selection is made', function () {
    LocaleSrv.setLocale('fr');
    expect(LocaleSrv.getLocale().value).toBe('Français');
    expect(LocaleSrv.getLocale().key).toBe('fr');
  });

  it('getLocales method should return all current locales in object', function () {
    expect(LocaleSrv.getLocales().length).toBe(6);
    expect(LocaleSrv.getLocales()[0].key).toBe('en');
    expect(LocaleSrv.getLocales()[1].key).toBe('de');
    expect(LocaleSrv.getLocales()[2].key).toBe('es');
    expect(LocaleSrv.getLocales()[3].key).toBe('fr');
    expect(LocaleSrv.getLocales()[4].key).toBe('it');
    expect(LocaleSrv.getLocales()[5].key).toBe('ja');
  });
  
});
