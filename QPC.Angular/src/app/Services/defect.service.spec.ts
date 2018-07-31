import { TestBed, inject } from '@angular/core/testing';

import { DefectService } from './defect.service';

describe('DefectService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DefectService]
    });
  });

  it('should be created', inject([DefectService], (service: DefectService) => {
    expect(service).toBeTruthy();
  }));
});
