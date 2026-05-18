import RestClient from './RestClient';
import { IBlogPost } from '@/types/blog-post.types';

class BlogPostRepository {
    public async getAll(): Promise<IBlogPost[]> {
        try {
            return await RestClient.get<IBlogPost[]>('/api/BlogPosts');
        } catch (error) {
            console.error('Failed to fetch all blog posts:', error);
            throw new Error('Could not retrieve blog posts. Please try again later.');
        }
    }

    public async getAllByBlog(blogSubFolder: string): Promise<IBlogPost[]> {
        try {
            return await RestClient.get<IBlogPost[]>(`/api/Blog/${blogSubFolder}/BlogPosts/All`);
        } catch (error) {
            console.error(`Failed to fetch posts for blog ${blogSubFolder}:`, error);
            throw new Error('Could not retrieve blog posts for this blog.');
        }
    }

    public async getById(blogSubFolder: string, id: number): Promise<IBlogPost> {
        try {
            return await RestClient.get<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost/${id}`);
        } catch (error) {
            console.error(`Failed to fetch post ${id} for blog ${blogSubFolder}:`, error);
            throw new Error('Could not retrieve the blog post.');
        }
    }

    public async save(blogSubFolder: string, blogPost: IBlogPost): Promise<IBlogPost> {
        try {
            const apiInput = {
                IsPublished: blogPost.IsPublished,
                Title: blogPost.Title,
                Text: blogPost.EntryText
            };

            if (blogPost.Id && blogPost.Id > 0) {
                return await RestClient.put<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost/${blogPost.Id}`, apiInput);
            } else {
                return await RestClient.post<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost`, apiInput);
            }
        } catch (error) {
            console.error(`Failed to save blog post in ${blogSubFolder}:`, error);
            throw new Error('Could not save the blog post. Please check your input.');
        }
    }

    public async delete(blogSubFolder: string, id: number): Promise<void> {
        try {
            return await RestClient.delete<void>(`/api/Blog/${blogSubFolder}/BlogPost/${id}`);
        } catch (error) {
            console.error(`Failed to delete post ${id} in blog ${blogSubFolder}:`, error);
            throw new Error('Could not delete the blog post.');
        }
    }
}

export default new BlogPostRepository();

