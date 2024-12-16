import { render as rtlRender } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';

export function render(ui: React.ReactElement, options = {}) {
  return rtlRender(ui, {
    wrapper: BrowserRouter,
    ...options,
  });
}

export * from '@testing-library/react';