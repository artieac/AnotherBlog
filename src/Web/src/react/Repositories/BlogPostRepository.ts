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
        console.log('Starting save process for blog post:', blogPost);
        // Map EntryText to Text if needed, or adjust API to accept EntryText
        // The API uses BlogPostInput which has 'Text'
        const apiInput = {
            IsPublished: blogPost.IsPublished,
            Title: blogPost.Title,
            Text: blogPost.EntryText
        };

        let savedPost: IBlogPost;
        try {
            if (blogPost.Id && blogPost.Id > 0) {
                console.log(`Sending PUT request to /api/Blog/${blogSubFolder}/BlogPost/${blogPost.Id}`);
                savedPost = await RestClient.put<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost/${blogPost.Id}`, apiInput);
            } else {
                console.log(`Sending POST request to /api/Blog/${blogSubFolder}/BlogPost`);
                savedPost = await RestClient.post<IBlogPost>(`/api/Blog/${blogSubFolder}/BlogPost`, apiInput);
            }
            console.log('Main post save successful. Saved post details:', savedPost);
        } catch (error) {
            console.error('Error saving main post:', error);
            throw error;
        }

        return savedPost;
    }

    public async delete(blogSubFolder: string, id: number): Promise<void> {
        return RestClient.delete<void>(`/api/Blog/${blogSubFolder}/BlogPost/${id}`);
    }
}

export default new BlogPostRepository();
