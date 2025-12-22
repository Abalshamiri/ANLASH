import { ANLASHTemplatePage } from './app.po';

describe('ANLASH App', function() {
  let page: ANLASHTemplatePage;

  beforeEach(() => {
    page = new ANLASHTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
