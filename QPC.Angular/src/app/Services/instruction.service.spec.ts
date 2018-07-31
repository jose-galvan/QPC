import { TestBed, inject } from '@angular/core/testing';

import { InstructionService } from './instruction.service';

describe('InstructionService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InstructionService]
    });
  });

  it('should be created', inject([InstructionService], (service: InstructionService) => {
    expect(service).toBeTruthy();
  }));
});
