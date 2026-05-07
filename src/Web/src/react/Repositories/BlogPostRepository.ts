import RestClient from './RestClient';
import { IBlogPost } from '../Models/IBlogPost';

class BlogPostRepository {
    public async getAll(): Promise<IBlogPost[]> {
        return RestClient.get<IBlogPost[]>('/api/BlogPosts');
    }

    public async getAllByBlog(blogSubFolder: string): Promise<IBlogPost[]> {
        return RestClient.get<IBlogPost[]>(`/api/Blog/${blogSubFolder}/BlogPosts/All`);
    }

    public async getById(blogSubFolder: string, id: number): Promise<IBlogPost> {
        return RestClient.get<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost/${id}`);
    }

    public async save(blogSubFolder: string, blogPost: any): Promise<IBlogPost> {
        // Map EntryText to Text if needed, or adjust API to accept EntryText
        // The API uses BlogPostInput which has 'Text'
        const apiInput = {
            IsPublished: blogPost.IsPublished,
            Title: blogPost.Title,
            Text: blogPost.EntryText,
            Tags: Array.isArray(blogPost.Tags) ? blogPost.Tags.map((t: any) => t.Name).join(',') : blogPost.Tags
        };

        if (blogPost.Id && blogPost.Id > 0) {
            return RestClient.put<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost/${blogPost.Id}`, apiInput);
        } else {
            return RestClient.post<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost`, apiInput);
        }
    }

    public async delete(blogSubFolder: string, id: number): Promise<void> {
        return RestClient.delete<void>(`/api/Blog/${blogSubFolder}/BlogPost/${id}`);
    }
}

export default new BlogPostRepository();
