import { describe, it, expect, vi } from 'vitest';
import { mount } from '@vue/test-utils';
import TaskManager from '../TaskManager.vue';

// Mock axios
vi.mock('axios', () => ({
  default: {
    get: vi.fn(() => Promise.resolve({ data: [
      { id: 1, title: 'Test Task', isCompleted: false }
    ] })),
    post: vi.fn(() => Promise.resolve({ data: { id: 2, title: 'New Task', isCompleted: false } })),
    patch: vi.fn(() => Promise.resolve()),
    delete: vi.fn(() => Promise.resolve()),
  }
}));

describe('TaskManager.vue', () => {
  it('renders the title and add button', () => {
    const wrapper = mount(TaskManager);
    expect(wrapper.text()).toContain('Task Manager');
    expect(wrapper.find('button').text()).toBe('Add Task');
  });

  it('renders tasks from API', async () => {
    const wrapper = mount(TaskManager);
    // Wait for fetchTasks to resolve
    await new Promise(resolve => setTimeout(resolve));
    expect(wrapper.text()).toContain('Test Task');
  });

  it('adds a new task', async () => {
    const wrapper = mount(TaskManager);
    await new Promise(resolve => setTimeout(resolve));
    await wrapper.find('input[type="text"]').setValue('New Task');
    await wrapper.find('button').trigger('click');
    await new Promise(resolve => setTimeout(resolve));
    expect(wrapper.text()).toContain('New Task');
  });
});
