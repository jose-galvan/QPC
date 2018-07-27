import { QPC.AngularPage } from './app.po';

describe('qpc.angular App', () => {
  let page: QPC.AngularPage;

  beforeEach(() => {
    page = new QPC.AngularPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
